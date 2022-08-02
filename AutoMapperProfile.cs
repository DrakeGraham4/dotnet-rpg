using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg
{
    // We have to create maps for mapping in order for AutoMapper to be able to map character type into GetCharacterDto which is organzied in Profile
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Creating map for character into GetCharacterDto
            CreateMap<Character, GetCharacterDto>();
            //Creating map for AddCharacterDto into Character to create Character
            CreateMap<AddCharacterDto, Character>();
            //Could use Automapper for update of chaarcter but you need to pay attention to the values that are being sent
            // CreateMap<UpdateCharacterDto, Character>();
        }
    }
}