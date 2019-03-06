using System.Collections.Generic;

namespace Fork.ViewModels.Project
{
    public class CreationViewModel
    {
        /// <summary>
        /// Creator's id.
        /// </summary>
        public int ActorId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.SemesterId"/>.
        /// </summary>
        public int SemesterId { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.ProjectName"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.ProjectHeadline"/>.
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.ProjectPitch"/>.
        /// </summary>
        public string Pitch { get; set; }

        /// <summary>
        /// See <see cref="BasicDataProject.Members"/>.
        /// </summary>
        public IEnumerable<int> Members { get; set; }
    }
}
