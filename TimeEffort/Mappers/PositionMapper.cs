using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TimeEffortCore.Entities;
using TimeEffort.Models;

namespace TimeEffort.Mappers
{
    public class PositionMapper
    {
        public static PositionViewModel MapPositionToModel(Position position)
        {
            return new PositionViewModel
            {
                Id = position.ID,
                Position = position.Name
            };
         
        }
        public static Position MapPositionFromModel(PositionViewModel model)
        {
            return new Position
            {
                ID = model.Id,
                Name = model.Position
            };
        }
        public static List<PositionViewModel> MapPositionsToModels(List<Position> list)
        {
                return list.Select(c => new PositionViewModel
                {
                    Id = c.ID,
                    Position = c.Name
                }).ToList();
        }
    }
}