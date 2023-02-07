using AngularAuthAPI.Data;
using AngularAuthAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace AngularAuthAPI.Helpers
{
    public class VaildationClass
    {
        private readonly AppDbContext _appDbContext;
        public VaildationClass(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> CheckUserNameExistAsync(string username)
        {
            return await _appDbContext.user.AnyAsync(x => x.Username == username);
        }
        public async Task<bool> CheckeEmailExistAsync(string email)
        {
            return await _appDbContext.user.AnyAsync(x => x.Email == email);
        }

        public async Task<bool> CheckUserCharAsync(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }
            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
           // string MatchEmailPattern = "^[a-zA-Z0-9]+$";
            if (!validateGuidRegex.IsMatch(password))
            {
                return false; 
            }
            return true;
        }

        public string DecodeFrom64(string encodedData)  
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        public string CreateJwtToke(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthentication@777"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: "https://localhost:7025", audience:"https://localhost:7025", 
            claims: new List<Claim>(), expires: DateTime.Now.AddMinutes(6),
            signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;                                                     
        }
    }
}
