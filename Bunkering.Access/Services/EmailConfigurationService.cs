using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace Bunkering.Access.Services
{
    public class EmailConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public EmailConfigurationService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
           
        }

        public async Task<ApiResponse> CreateEmailConfiguration(EmailConfigurationViewModel model)
        {
            var createEmail = await _unitOfWork.EmailConfiguration.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (createEmail == null)
            {
                var email = new EmailConfiguration
                {
                    Name = model.Name,
                    Email = model.Email,

                };

                await _unitOfWork.EmailConfiguration.Add(email);
                await _unitOfWork.SaveChangesAsync("");

                _response = new ApiResponse
                {
                    Data = email,
                    Message = "Email Created Successfully",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true
                };
            }
            else
            {
                _response = new ApiResponse
                {
                   
                    Message = "Email Already Exist",
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    Success = false
                };
            }

            return _response;
        }

        public async Task<ApiResponse> UpdateEmailConfiguration(EmailConfigurationViewModel model)
        {
            var update = await _unitOfWork.EmailConfiguration.FirstOrDefaultAsync(x => x.Name == model.Name);
            if(update != null)
            {
                update.Email = model.Email;

                await _unitOfWork.EmailConfiguration.Update(update);
                _unitOfWork.Save();

                _response = new ApiResponse
                {

                    Message = "Email Updated Successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };


            }
            else
            {
                _response = new ApiResponse
                {

                    Message = "Name does not Exist",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Success = false
                };

            }
            return _response;

        }

        public async Task<ApiResponse> DeleteEmailConfiguration(int id)
        {
            var delEmail = await _unitOfWork.EmailConfiguration.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if(delEmail != null)
            {
                if(delEmail.IsActive)
                {
                    delEmail.IsActive = false;

                    await _unitOfWork.EmailConfiguration.Update(delEmail);
                    _unitOfWork.Save();


                    _response = new ApiResponse
                    {

                        Message = "Email has been deleted",
                        Data = delEmail,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
                else
                {
                    _response = new ApiResponse
                    {

                        Message = "Email has already been deleted",
                        StatusCode = HttpStatusCode.Conflict,
                        Success = false,
                    };

                }
                
            }
            else
            {
                _response = new ApiResponse
                {

                    Message = "Email does not Exist",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };


            }
            return _response;
        }

        public async Task<ApiResponse> AllEmailConfigurations()
        {
            var allEmail = await _unitOfWork.EmailConfiguration.Find(c => c.IsActive);

            _response = new ApiResponse
            {
                Data = allEmail,
                Message = "Successful",
                StatusCode = HttpStatusCode.OK,
                Success = true,
            };

            return _response;
        }
    }
}
