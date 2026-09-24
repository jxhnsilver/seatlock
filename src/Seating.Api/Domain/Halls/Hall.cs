using BuildingBlocks.Exceptions;

namespace Seating.Api.Domain.Halls
{
    /// <summary>
    /// Доменная сущность кинозала.
    /// </summary>
    public sealed class Hall
    {
        /// <summary>
        /// Уникальный идентификатор зала.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Отображаемое название зала.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Текущий формат зала.
        /// </summary>
        public HallType Type { get; private set; }

        /// <summary>
        /// Текущий операционный статус зала.
        /// </summary>
        public HallStatus Status { get; private set; }

        public Hall(string name, HallType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvariantViolationException("Название зала не может быть пустым.");

            Name = name;
            Type = type;
            Status = HallStatus.Active;
        }

        /// <summary>
        /// Устанавливает новое название для зала.
        /// </summary>
        /// <param name="newName">Новое название зала.</param>
        public void SetName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new InvariantViolationException("Название зала не может быть пустым.");

            Name = newName;
        }

        public void SetType(HallType newType)
        {
            Type = newType;
        }

        /// <summary>
        /// Изменяет текущий операционный статус зала.
        /// </summary>
        /// <param name="newStatus">Новый операционный статус зала.</param>
        public void ChangeStatus(HallStatus newStatus)
        {
            Status = newStatus;
        }
    }
}
