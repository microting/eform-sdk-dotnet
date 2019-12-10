using System;

namespace Microting.eForm.Dto
{
    public class WorkerDto
    {
        #region con
        public WorkerDto()
        {
        }

        public WorkerDto(int workerUId, string firstName, string lastName, string email, DateTime? createdAt, DateTime? updatedAt)
        {
            if (firstName == null)
                firstName = "";

            if (lastName == null)
                lastName = "";

            if (email == null)
                email = "";

            WorkerUId = workerUId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
        #endregion

        #region var
        /// <summary>
        ///...
        /// </summary>
        public int WorkerUId { get; }

        /// <summary>
        ///...
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string LastName { get; }

        /// <summary>
        ///...
        /// </summary>
        public string Email { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        ///...
        /// </summary>
        public DateTime? UpdatedAt { get; }

        #endregion

        public override string ToString()
        {
            return "WorkerUId:" + WorkerUId + " / FirstName:" + FirstName + " / LastName:" + LastName + " / Email:" + Email + " / CreatedAt:" + CreatedAt + " / UpdatedAt:" + UpdatedAt + ".";
        }
    }
}