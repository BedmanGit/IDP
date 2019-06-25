using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.ID, opt =>
                {
                    opt.MapFrom(d => d.UserId);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => DateTime.TryParse(d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "dob")?.ClaimValue, out DateTime dob) ? dob.CalculateAge() : default(int));
                })
                .ForMember(dest => dest.Gender, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "gender").ClaimValue);
                })
                .ForMember(dest => dest.City, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "city").ClaimValue);
                })
                .ForMember(dest => dest.Country, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "country").ClaimValue);
                })
                .ForMember(dest => dest.KnownAs, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "knownas").ClaimValue);
                })
                .ForMember(dest => dest.Interests, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "interests").ClaimValue);
                })
                .ForMember(dest => dest.Introduction, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "introduction").ClaimValue);
                })
                .ForMember(dest => dest.LookingFor, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "lookingfor").ClaimValue);
                });

            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.ID, opt =>
                {
                    opt.MapFrom(d => d.UserId);
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                { 
                    opt.ResolveUsing(d => DateTime.TryParse(d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "dob")?.ClaimValue, out DateTime dob) ? dob.CalculateAge() : default(int)); 
                })
                .ForMember(dest => dest.Gender, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "gender").ClaimValue);
                })
                .ForMember(dest => dest.City, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "city").ClaimValue);
                })
                .ForMember(dest => dest.Country, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "country").ClaimValue);
                })
                .ForMember(dest => dest.KnownAs, opt =>
                {
                    opt.MapFrom(d => d.Claims.FirstOrDefault(c => c.ClaimType.ToLower() == "knownas").ClaimValue);
                })
                ;
            CreateMap<Photo, PhotoForDetailDTO>();
            CreateMap<List<UserClaim>, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDTO>();
        }
    }
}