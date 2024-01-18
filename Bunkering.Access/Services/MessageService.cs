using Azure;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class MessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string User;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;
        private readonly UserManager<ApplicationUser> _user;
        public MessageService(IUnitOfWork unitOfWork,  
            IHttpContextAccessor contextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            User = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            _user = userManager;
        }

        public async Task<ApiResponse> CreateMessageAsync(MessageModel model)
        {
            try
            {

                var Message = new Message
                {
                    ApplicationId = model.ApplicationId,
                    Content = model.Content,
                    Subject = model.Subject,
                    UserId = model.UserId,
                    Date = DateTime.Now.AddHours(1),

                };

                _unitOfWork.Message.Add(Message);
                var save = await _unitOfWork.SaveChangesAsync(model.UserId);


                if (save > 0)
                {
                    _response = new ApiResponse
                    {
                        Message = "Message created successfully",
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                        Data=Message
                    
                    };
                }

                else
                    _response = new ApiResponse
                    {
                        Message = "Not successful",
                        StatusCode = HttpStatusCode.OK,
                        Success = false
                    };
            }
            catch (Exception ex)
            {
                _response = new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false
                };
            }
            return _response;
        }


        public async Task<ApiResponse> GetAllMessages()
        {
            var user = await _user.FindByEmailAsync(User);
            var messages = (await _unitOfWork.Message.Find(x => x.UserId.Equals(user.Id))).OrderByDescending(x => x.Id);
            if (messages != null)
                _response = new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = messages.Select(d => new
                    {
                        d.Id,
                        d.Subject,
                        d.Content,
                        d.ApplicationId,
                        d.UserId,
                        d.Date,
                        d.IsRead
                    })
                };
            else
                _response = new ApiResponse
                {
                    Message = "No Message found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return _response;
        }


        public async Task<ApiResponse> GetAllMessagesByCompId(string userId)
        {
           
            var messages = (await _unitOfWork.Message.Find(x => x.UserId.Equals(userId))).OrderByDescending(x => x.Id);
            if (messages != null)
                _response = new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = messages.Select(d => new
                    {
                        d.Id,
                        d.Subject,
                        d.Content,
                        d.ApplicationId,
                        d.UserId,
                        d.Date,
                        d.IsRead
                    })
                };
            else
                _response = new ApiResponse
                {
                    Message = "No Message found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return _response;
        }



        public async Task<ApiResponse> GetMessageById(int id)
        {
            var messages = await _unitOfWork.Message.FirstOrDefaultAsync(m => m.Id == id);
            if (messages != null)
            {
               
                _response = new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = messages
                };
                messages.IsRead = true;
                await _unitOfWork.Message.Update(messages);
                _unitOfWork.Save();

            }              
                
            else
                _response = new ApiResponse
                {
                    Message = "No Message found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return _response;
        }



        public async Task<ApiResponse> DeleteMessage(int id)
        {
            try
            {
                var save = 0;
                var message = await _unitOfWork.Message.FirstOrDefaultAsync(m => m.Id == id);
                if (message != null)
                {
                    await _unitOfWork.Message.Remove(message);
                    save = _unitOfWork.Save();
                }
                if (save > 0)
                {
                    return new ApiResponse
                    {
                        Message = "Deleted Successful",
                        StatusCode = HttpStatusCode.OK,
                        Success = true
                    };
                }
            }

            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = ex.Data,
                    Success = false
                };
            }

            return new ApiResponse
            {
                Message = "Error in operation",
                StatusCode = HttpStatusCode.BadRequest,
                Success = false
            };

        }




        public async Task<ApiResponse> Read(int id)
            {
              var message = await _unitOfWork.Message.FirstOrDefaultAsync(a=>a.Id == id);

            if(message != null)
            {
                message.IsRead = true;
                await _unitOfWork.Message.Update(message);
                _unitOfWork.Save();

                return new ApiResponse
                {
                    Message = "Read",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
            }
            else
                _response = new ApiResponse
                {
                    Message = "Message Not Read",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };

            return  _response;

        }
    }
}
