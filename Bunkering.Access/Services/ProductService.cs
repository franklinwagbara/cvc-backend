using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net;

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
        }

        public async Task<ApiResponse> AllProducts()
        {
            try
            {
                var allProducts = await _unitOfWork.Product.Find(x => !x.IsDeleted);
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
                return new ApiResponse { Message = ex.Message };
            }
        }

        public async Task<ApiResponse> GetProductsById(int id)
        {
            var getById = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if(getById != null)
            {
                _response = new ApiResponse
                {
                    Message = "Successful",
                    Data = getById,
                    StatusCode = HttpStatusCode.OK,
                    Success = true

                };
            }
            else
            {
                _response = new ApiResponse
                {

                    Message = "Product Not Found",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
            return _response;
        }

        public async Task<ApiResponse> CreateProduct(ProductViewModel model)
        {
            var createProduct = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Name.ToLower() == model.Name.ToLower());
            if (createProduct != null)
            {
                _response = new ApiResponse
                {
                    Message = "Product already exist",
                    StatusCode = HttpStatusCode.Conflict,
                    Success = false
                };
                return _response;
            }

            var product = new Product
            {
                Name = model.Name,
                ProductType = model.ProductType,
                RevenueCode = model.RevenueCode,
                RevenueCodeDescription = model.RevenueCodeDescription,
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
                editProduct.Name = model.Name;
                editProduct.ProductType = model.ProductType;
                editProduct.RevenueCodeDescription = model.RevenueCodeDescription;
                editProduct.RevenueCode = model.RevenueCode;

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

        public async Task<ApiResponse> DeleteProduct(int id)
        {
            var delProduct = await _unitOfWork.Product.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (delProduct != null)
            {
                if (!delProduct.IsDeleted)
                {
                    delProduct.IsDeleted = true;
                    await _unitOfWork.Product.Update(delProduct);
                    _unitOfWork.Save();

                    _response = new ApiResponse
                    {
                        Message = "Successful",
                        Data = delProduct,
                        StatusCode = HttpStatusCode.OK,
                        Success = true,
                    };
                }
                else
                {
                    _response = new ApiResponse
                    {
                        Message = "Product already deleted",
                        StatusCode = HttpStatusCode.BadRequest,
                        Success = true,
                    };
                }
            }
            else
            {
                _response = new ApiResponse
                {
                    Message = "Product doesnt exist",
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false,
                };
            }
            return _response;
        }
    }
}
