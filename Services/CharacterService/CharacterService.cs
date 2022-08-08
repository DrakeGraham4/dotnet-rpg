using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        //Maps the objects by creating new instance of the necessary class and then setting every property one by one, automapper does this for us
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));
        

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
             //Find new character based on the Dto
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            //Adding new character to dbCharacters list
            _context.Characters.Add(character);
            //To make changes happen we must call this method. This method writes the changes to the database and generates new id for the character
            await _context.SaveChangesAsync();

            //Maps the whole characters list and give it to ServiceResponse by using the select method followed by a lambda expression where we map every character object of the list into a character Dto
            serviceResponse.Data = await _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();

            try
            {   
            Character character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            if(character != null)
            {
            //Removes character and sends back list of characters that are left
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            response.Data = _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            else
            {
                response.Success = false;
                response.Message = "Character not found";
            }
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
            //Intitialize service response, grab all characters from the db and map these characters, return service response
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
                .Where(c => c.User.Id == GetUserId())
                .ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            //returns first character from database(db) where Id of dbCharacter = given Id
            var dbCharacter = await _context.Characters
                //Checking character id and user id
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

            //Using map function to first decide in <> which type the value (dbCharacter) parameter should be mapped to
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            //Start with Service Response and then try to find the rpgcharacter with the given id of the updated character in the characters list
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

            try
            {   
            //Access the characters of the DataContext and search the characters asynchronously
            var character = await _context.Characters
            //Include user property
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

            if(character.User.Id == GetUserId())
            {

            //Could use automapper to make all changes below but need to understand what you are sending
            // _mapper.Map(updatedCharacter, character);

            // Override almost every property of the character one by one
            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.Strength;
            character.Defense = updatedCharacter.Defense;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Class = updatedCharacter.Class;
            }
            else
            {
                response.Success = false;
                response.Message = "Character not found";
            }
            //Saves to db
            await _context.SaveChangesAsync();

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