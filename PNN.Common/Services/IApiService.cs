﻿using PNN.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PNN.Common.Services
{
    public interface IApiService
    {
        Task<Response<UserResponse>> GetOwnerByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            string email);

        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            TokenRequest request);

        Task<bool> CheckConnectionAsync(String url);

        Task<Response<PublicationsResponse>> GetContentsAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<Response<object>> RecoverPasswordAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            EmailRequest emailRequest);


        Task<Response<object>> RegisterUserAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            UserRequest userRequest);


        Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken);

        Task<Response<object>> PostAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken);

        Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            T model,
            string tokenType,
            string accessToken);

        Task<Response<object>> ChangePasswordAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            ChangePasswordRequest changePasswordRequest,
            string tokenType,
            string accessToken);

        Task<Response<object>> AddComment(
           string urlBase, 
           string servicePrefix, 
           string controller, 
           CommentRequest comment, 
           string tokenType, 
           string accessToken);

        Task<Response<object>> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);

        Task<Response<object>> GetLastContentByUserId(
                string urlBase,
                string servicePrefix,
                string controller,
                string tokenType,
                string accessToken,
                string userId);

        Task<Response<object>> DeleteAsync(
                string urlBase,
                string servicePrefix,
                string controller,
                int id,
                string tokenType,
                string accessToken);


    }

}
