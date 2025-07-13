using DAL.Contracts;
using DTO.UserDTO;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class UserDomain : IUserDomain
    {
        private readonly IUserRepository _userRepository;

        public UserDomain(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                FatherName = user.FatherName,
                LastName = user.LastName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                PersonalId = user.PersonalId
            };
        }
    }

}
