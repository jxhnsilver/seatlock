namespace Seating.Api.Domain.Halls
{
    /// <summary>
    /// Определяет операционный статус кинозала.
    /// </summary>
    public enum HallStatus
    {
        /// <summary>
        /// Зал активен и доступен для планирования сеансов и продажи билетов.
        /// </summary>
        Active = 0,

        /// <summary>
        /// Зал временно недоступен из-за проведения технических или сервисных работ.
        /// </summary>
        Maintenance = 1,

        /// <summary>
        /// Зал архивирован или выведен из эксплуатации.
        /// </summary>
        Inactive = 2
    }
}
