﻿using CrossesZeroes.Abstractions;
using CrossesZeroes.Common;
using CrossesZeroes.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CrossesZeroes.Classes
{
    /// <summary>
    /// Игрок-компьютер
    /// </summary>
    public class AiPlayer : IAiPlayer
    {
        private readonly AiPlayerBehaviour behaviour;
        private readonly ILogger<AiPlayer> logger;
        private ICrossesZeroesField? field;

        public AiPlayer(IOptions<AiPlayerBehaviour> behaviour, ILogger<AiPlayer> logger)
        {
            this.behaviour = behaviour.Value;
            this.logger = logger;
        }

        public void ReportEnd(bool victory)
        {
            logger.LogInformation("Reporting end, victory: {Victory}", victory);
        }

        public void Init(CellState mark, ICrossesZeroesField field)
        {
            logger.LogInformation("Initialization with mark: {Mark}", mark);

            this.field = field;
        }

        public Task<Point> Turn()
        {
            logger.LogInformation("Turning");

            //Перебор всех клеток в поиске свободных
            for (int i = 0; i < field!.Height; i++)
                for (int j = 0; j < field.Width; j++)
                    if (field[i, j] == CellState.Empty)
                    {
                        Point point = new(i, j);

                        logger.LogDebug("Turning result: {Point}", point);
                        return Task.FromResult(point);
                    }

            logger.LogMessageAndThrow(new InvalidProgramException("No one empty cells"));
            throw new Exception();
        }

        public Task<bool> IsRepeatWanted()
        {
            logger.LogInformation("Repeat wanted: {RepeatWanted}", behaviour.wantRepeat);

            return Task.FromResult(behaviour.wantRepeat);
        }

        public void NotifyFieldChange(Point point)
        {
            logger.LogInformation("Field changed at {Point}", point);
        }

        public class AiPlayerBehaviour
        {
            public bool wantRepeat;
        }
    }
}
