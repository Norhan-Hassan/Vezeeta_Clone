namespace Vezeeta_Clone.Data.Entities.Enums
{
    public enum AppointmentStatus
    {
        Upcoming = 1, // The appointment is scheduled but not yet occurred
        Pending = 2, // The appointment is awaiting confirmation 
        Completed = 3,// The appointment has confirmed at the side of the patient
        Confirmed = 4, // appointment has been confirmed by patient through success payment
        Cancelled = 5 // The appointment has been cancelled by the patient 
    }
}
