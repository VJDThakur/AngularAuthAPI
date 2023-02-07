using AngularAuthAPI.Data;
using AngularAuthAPI.Helpers;
using AngularAuthAPI.IUsers;
using AngularAuthAPI.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace AngularAuthAPI.Repository
{
    public class UserRepository : Iuser
    {
        private readonly AppDbContext _appDbContext;
        private readonly VaildationClass _function;
        public UserRepository(AppDbContext appDbContext,VaildationClass functionscls)
        {
            _appDbContext = appDbContext;
            _function = functionscls;
        }

        public async Task<ResponseClass> Autheticate(User userobj)
        {
            ResponseClass responseClass = null;

            var user = await _appDbContext.user.FirstOrDefaultAsync(x => x.Username == userobj.Username);
            if (user == null)
            {  

                responseClass = new ResponseClass
                {
                    statusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Username or password not exist",
                };
                return await Task.Run(() => responseClass);
            }


            if (!PasswordHasher.VerifyPassword(userobj.Username, user.PassWords))

                user.Token = _function.CreateJwtToke(user);
                responseClass = new ResponseClass
                {
                   statusCode = System.Net.HttpStatusCode.OK,
                   Message = "User Login succefully!",
                   data = user.Token 
                }; 

                 return await Task.Run(() => responseClass); 
        }

        public async Task<ResponseClass> RagisterUser (User userobj)
        {
            ResponseClass responseClass = null;
            try
            {
                if (await _function.CheckUserNameExistAsync(userobj.Username))
                {
                    responseClass = new ResponseClass
                    {
                        Message = "UserName Already Exiext",
                        statusCode = System.Net.HttpStatusCode.BadRequest
                    };
                    return await Task.Run(() => responseClass);
                } 
                if (await _function.CheckUserNameExistAsync(userobj.Email))
                {
                    responseClass = new ResponseClass
                    {
                        statusCode = System.Net.HttpStatusCode.BadGateway,
                        Message = "EmailId Alredy Exiest"
                    };
                    return await Task.Run(()=> responseClass);
                }
                
                var isPasswordValid = await _function.CheckUserCharAsync(userobj.PassWords);
                if(!isPasswordValid)
                {
                    responseClass = new ResponseClass
                    {
                        statusCode = System.Net.HttpStatusCode.BadGateway,
                        Message = " PassWord is not vaild should Shuld be alfa numbric and also carry 8 car and with special charector",
                       
                    };
                    return await Task.Run(() => responseClass);
                }

               userobj.PassWords = PasswordHasher.HasPassword(userobj.PassWords);

                await _appDbContext.user.AddAsync(userobj);
                userobj.Token = "";
                _appDbContext.SaveChanges();

                responseClass = new ResponseClass
                {
                    statusCode = System.Net.HttpStatusCode.OK,
                   Message = "Successfully creating records"
                };
            }

            catch (Exception ex)  
            {
                responseClass = new ResponseClass
                {
                    statusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
              return await Task.Run(() => responseClass);

        }
    }
}
