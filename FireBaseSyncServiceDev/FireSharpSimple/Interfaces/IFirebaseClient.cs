﻿using System;
using System.Threading.Tasks;
//using FireSharp.EventStreaming;
using FireSharpSimple.Response;

namespace FireSharpSimple.Interfaces
{
    public interface IFirebaseClient
    {
        Task<FirebaseResponse> GetAsync(string path);
        Task<FirebaseResponse> GetAsync(string path, QueryBuilder queryBuilder);

//        Task<EventRootResponse<T>> OnChangeGetAsync<T>(string path, ValueRootAddedEventHandler<T> added = null);
        Task<SetResponse> SetAsync<T>(string path, T data);
        Task<PushResponse> PushAsync<T>(string path, T data);
        Task<FirebaseResponse> DeleteAsync(string path);
        Task<FirebaseResponse> UpdateAsync<T>(string path, T data);
        FirebaseResponse Get(string path, QueryBuilder queryBuilder);
        FirebaseResponse Get(string path);
        SetResponse Set<T>(string path, T data);
        PushResponse Push<T>(string path, T data);
        FirebaseResponse Delete(string path);
        FirebaseResponse Update<T>(string path, T data);
        FirebaseResponse CreateUser(string email, string password);
        FirebaseResponse ChangeEmail(string oldEmail, string password, string newEmail);
        FirebaseResponse RemoveUser(string email, string password);
        FirebaseResponse ResetPassword(string email, string password);
        FirebaseResponse ChangePassword(string email, string oldPassword, string newPassword);


//        Task<EventStreamResponse> OnAsync(string path,
//            ValueAddedEventHandler added = null,
//            ValueChangedEventHandler changed = null,
//            ValueRemovedEventHandler removed = null,
//            object context = null);
    }
}