using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Bunkering.Access
{
	public class Seeder
	{
		private readonly ApplicationContext _db;
		private readonly UserManager<ApplicationUser> _userMnager;
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly IServiceProvider _serviceProvider;
		private readonly IElps _elps;

		public Seeder(
			ApplicationContext db,
			IServiceProvider serviceProvider,
			UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			IElps elps)
		{
			_db = db;
			_serviceProvider = serviceProvider;
			_userMnager = userManager;
			_roleManager = roleManager;
			_elps = elps;


		}

		public async Task CreateRoles()
		{
			var roles = new[]
				{
					RoleConstants.COMPANY,
					RoleConstants.REVIEWER,
					RoleConstants.SUPER_ADMIN,
					RoleConstants.SUPERVISOR,
					RoleConstants.COORDINATOR,
					RoleConstants.Field_Officer,
					RoleConstants.FAD
				};
			foreach (var role in roles)
			{
				;
				if (!await _roleManager.RoleExistsAsync(role))
					await _roleManager.CreateAsync(new ApplicationRole { Name = role, Description = role});
			}
			_db.SaveChanges();
		}

		public async Task CreateAdmin()
		{
			var user = await _userMnager.FindByEmailAsync("damilare.olanrewaju@brandonetech.com");

			if (user == null)
			{
				var staff = _elps.GetStaff("damilare.olanrewaju@brandonetech.com");
				if (staff != null)
				{
					user = new ApplicationUser
					{
						Email = staff.Email.ToLower(),
						UserName = staff.Email.ToLower(),
						FirstName = staff.FirstName,
						LastName = staff.LastName,
						PhoneNumber = staff.PhoneNo,
						IsActive = true,
						ElpsId = staff.Id,
						CreatedBy = "system",
						CreatedOn = DateTime.UtcNow.AddHours(1),
					};
					var result = await _userMnager.CreateAsync(user);

					if (result.Succeeded)
						await _userMnager.AddToRoleAsync(user, "SuperAdmin");

				}
			}
		}

		public async Task CreateStates()
		{
			var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
			var country = await _context.Countries.ToListAsync();

			var client = new HttpClient();
			if (!country.Any())
			{
				var request = new HttpRequestMessage
				{
					Method = HttpMethod.Get,
					RequestUri = new Uri("https://countriesnow.space/api/v0.1/countries/states")
				};
				using (var response = await client.SendAsync(request))
				{
					if (response.StatusCode.Equals(HttpStatusCode.OK))
					{
						var states = new List<State>();
						var countryObj = new List<Country>();
						var content = await response.Content.ReadAsStringAsync();
						var obj = content.Parse<Dictionary<string, object>>();
						var countrylist = obj.GetValue("data");
						var countries = countrylist.Stringify().Parse<List<Dictionary<string, object>>>();

						countries.ForEach(c =>
						{
							var cl = new Country { Code = (string)c.GetValue("iso3"), Name = (string)c.GetValue("name") };
							_context.Countries.Add(cl);
							_context.SaveChanges();
							countryObj.Add(cl);

							var statelist = c.GetValue("states").Stringify().Parse<List<Dictionary<string, string>>>();
							statelist.ForEach(s =>
							{
								var code = s.GetValue("state_code");
								states.Add(new State
								{
									CountryId = cl.Id,
									Name = s.GetValue("name"),
									Code = !string.IsNullOrEmpty(code) ? code : ""
								});
							});
						});

						if (states.Count > 0)
						{
							_context.States.AddRange(states.OrderBy(x => x.Name));
							_context.SaveChanges();
						}

						states.Where(y => y.CountryId.Equals(countryObj.FirstOrDefault(j => j.Name.ToLower().Equals("nigeria")).Id)).ToList().ForEach(x =>
						{
							var req = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://api.facts.ng/v1/states/{x.Name.ToLower()}")).Result;
							if (req.IsSuccessStatusCode)
							{
								var lgaOutput = req.Content.ReadAsStringAsync().Result;
								var lgaDic = lgaOutput.Parse<Dictionary<string, object>>();
								var lgalist = lgaDic.GetValue("lgas").Stringify().Parse<List<string>>();
								var lgaObj = new List<LGA>();

								lgalist.ForEach(l =>
								{
									lgaObj.Add(new LGA
									{
										Code = "",
										Name = l,
										StateId = x.Id
									});
								});
								_context.LGAs.AddRange(lgaObj);
								_context.SaveChanges();

							}
						});
					}
				}
			}
			else
			{
				var n = country.FirstOrDefault(x => x.Name.ToLower().Equals("nigeria"));
				if (n != null)
				{
					var st = await _context.States.Where(x => x.CountryId.Equals(n.Id)).ToListAsync();
					var stateIds = st.Select(x => x.Id);
					var lg = _context.LGAs.Where(l => stateIds.Contains(l.StateId)).ToList();
					if (st.Any() && lg.Count == 0)
					{
						st.ForEach(x =>
						{
							var name = x.Name.Contains(" State") ? x.Name.Split(" State")[0] : x.Name;
							var req = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://api.facts.ng/v1/states/{name.ToLower()}")).Result;
							if (req.IsSuccessStatusCode)
							{
								var lgaOutput = req.Content.ReadAsStringAsync().Result;
								var lgaDic = lgaOutput.Parse<Dictionary<string, object>>();
								var lgalist = lgaDic.GetValue("lgas").Stringify().Parse<List<string>>();
								var lgaObj = new List<LGA>();

								lgalist.ForEach(l =>
								{
									lgaObj.Add(new LGA
									{
										Code = "",
										Name = l,
										StateId = x.Id
									});
								});
								_context.LGAs.AddRange(lgaObj);
								_context.SaveChanges();

							}
						});
					}
				}
			}
		}

		public async Task CreateDefaultFcailityTypes()
		{
			var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
			var factypes = new[] { "Mobile", "Fixed" };
			if (!_context.FacilityTypes.Any())
			{
				foreach (var f in factypes)
					_context.FacilityTypes.Add(new FacilityType { Name = f, Code = f.StartsWith("M") ? "M" : "F" });

				_context.SaveChanges();
			}
		}

		public async Task CreateAppTypes()
		{
			var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
			//var apptypes = new[] { "NOA", "COQ", "DebitNote" };
			var apptypes = EnumExtension.GetNames<AppTypes>();
			var appTypesDb = apptypes.Where(x => !_context.ApplicationTypes.Any(a => a.Name.Equals(x)));

            if (appTypesDb != null)
			{
				foreach (var f in appTypesDb)
					_context.ApplicationTypes.Add(new ApplicationType { Name = f });

				_context.SaveChanges();
			}
		}

        public async Task CreateTankMeasurementTypes()
        {
            var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
            //var apptypes = new[] { "NOA", "COQ", "DebitNote" };
            var mtypes = EnumExtension.GetNames<MeasureType>();
            var mTypesDb = mtypes.Where(x => !_context.MeasurementTypes.Any(a => a.Name.Equals(x)));

            if (mTypesDb != null)
            {
                foreach (var f in mTypesDb)
                    _context.MeasurementTypes.Add(new MeasurementType { Name = f });

                _context.SaveChanges();
            }
        }

        public async Task CreateVesselType()
		{
			var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
			var vesselType = new[] { "Barge", "Vessel" };
			if (!_context.VesselTypes.Any())
			{
				foreach (var v in vesselType)
					_context.VesselTypes.Add(new VesselType { Name = v });
				_context.SaveChanges();
			}

		}

		public async Task CreateProducts()
		{
			var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
			var products = new[] { "Fuel Oils", "AGO", "Lubricationg Oils" };
			if (!_context.Products.Any())
			{
				foreach (var f in products)
					_context.Products.Add(new Product { Name = f, ProductType = "NonGas" });

				_context.SaveChanges();
			}
		}

		public async Task CreateLocations()
		{
			var _context = _serviceProvider.GetRequiredService<ApplicationContext>();
			var location = new[] { "HQ", "FO", "ZO" };
			if (!_context.Locations.Any())
			{
				foreach (var l in location)
					_context.Locations.Add(new Location { Name = l });
				_context.SaveChanges();
			}
		}
	}
}



