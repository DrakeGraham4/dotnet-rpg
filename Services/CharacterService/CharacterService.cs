using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;


namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character(),
            new Character { Id = 1, Name = "Sam"}
        };

        //Maps the objects by creating new instance of the necessary class and then setting every property one by one, automapper does this for us
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }
        

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
             //Find new character based on the Dto
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            //Set correct id by finding the current Max value in the characters list, increasing by 1
            character.Id = characters.Max(c => c.Id) + 1;

            //Adding new character to characters list
            characters.Add(character);

            //Maps the whole characters list and give it to ServiceResponse by using the select method followed by a lambda expression where we map every character object of the list into a character Dto
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();

            try
            {   
            Character character = characters.First(c => c.Id == id);

            //Removes character and sends back list of characters that are left
            characters.Remove(character);
            response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            
            }
            catch (Exception ex)
            {
            response.Success = false;
            response.Message = ex.Message;
            }

            //Return Service Response
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDto>> 
            {
                Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
            };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            //returns first character where Id of character = given Id
            var character = characters.FirstOrDefault(c => c.Id == id);

            //Using map function to first decide in <> which type the value (character) parameter should be mapped to
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            //Start with Service Response and then try to find the rpgcharacter with the given id of the updated character in the characters list
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

            try
            {   
            Character character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

            //Could use automapper to make all changes below but need to understand what you are sending
            // _mapper.Map(updatedCharacter, character);

            // Override almost every property of the character one by one
            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Class = updatedCharacter.Class;

            //Use automapper to map the updated character to the GetCharacterDto
            response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
            response.Success = false;
            response.Message = ex.Message;
            }

            //Return Service Response
            return response;

        }
    }
}