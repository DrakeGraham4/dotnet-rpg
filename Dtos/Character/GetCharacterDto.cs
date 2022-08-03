using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Dtos.Character
{
    /*Idea behind Dtos is that you've got smaller objects that do not consist of every property of the corresponding model
     Example: When we create a database table for our Rpg characters later, we could add properties for the create or modified date or a flag for a soft deletion of that character, we don't want to send that data to the client so we map certain properties of the model to the Dto */
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Frodo";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Halfling;
    }
}