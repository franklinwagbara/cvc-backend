using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bunkering.Access.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public ProductService(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
           // _response = new ApiResponse();
        }

        public async Task<ApiResponse> AllProducts()
        {
            try
            {
                var allProducts = await _unitOfWork.Product.GetAll();
                return new ApiResponse
                {
                    Data = allProducts,
                    Message = "Successful",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse { Data = ex };
            }

        }
        public async Task<ApiResponse> CreateProduct(ProductViewModel model)
        {
            var createProduct = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());
            if (createProduct != null)
            {
                _response = new ApiResponse
                {
                    Message = "Product already exist",
                    StatusCode = HttpStatusCode.OK,
                    Success = true
                };
                return _response;
            }

            var product = new Product
            {
                Name = model.Name,
                ProductType = model.ProductType
            };

            await _unitOfWork.Product.Add(product);
            await _unitOfWork.SaveChangesAsync("");
            model.Id = product.Id;

            _response = new ApiResponse
            {
                Data = model,
                Message = "Product Created",
                StatusCode = HttpStatusCode.OK,
                Success = true
            };

            return _response;
        }
        public async Task<ApiResponse> EditProduct(ProductViewModel model)
        {
            var editProduct =  await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (editProduct != null)
            {
                model.Name = editProduct.Name;
                model.ProductType = model.ProductType;

                await _unitOfWork.Product.Update(editProduct);
                 _unitOfWork.Save();

                _response = new ApiResponse
                {
                    Message = "Updated Successfully",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                };

            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "Product Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }
            return _response;
        }
      

      
    }
}
