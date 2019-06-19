using System;
using System.Collections.Generic;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("Applciation-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Allow-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Today.Year - dateTime.Year;
            if (dateTime.AddYears(age) > DateTime.Today)
            {
                age --;
            }
            return age;
        }

        public static List<UserClaim> ToClaims(this UserForUpdateDTO userUpdateDTO)
        {
            List<UserClaim> claims = new List<UserClaim>();
            Type t = userUpdateDTO.GetType();
            foreach (var p in t.GetProperties())
            {
                claims.Add(new UserClaim(p.Name, p.GetValue(userUpdateDTO).ToString()));
            }
            return claims;
            
        }
    }
}