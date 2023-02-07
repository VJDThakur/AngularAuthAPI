using AngularAuthAPI.Model;

namespace AngularAuthAPI.IUsers
{
    public interface Iuser
    {
         Task<ResponseClass> Autheticate (User userobj);
        Task<ResponseClass> RagisterUser  (User userobj);
    }
}
