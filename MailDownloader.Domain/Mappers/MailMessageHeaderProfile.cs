using AutoMapper;
using MailDownloader.Domain.Models;
using Limilabs.Mail;
using System.Linq;

namespace MailDownloader.Domain.Mappers
{
    internal class MailMessageHeaderMapper : Profile
    {
        public MailMessageHeaderMapper()
        {
            CreateMap<IMail, MailMessageHeader>()
                .ForMember(d => d.Subject, opt => opt.MapFrom(t => t.Subject))
                .ForMember(d => d.From, opt => opt.MapFrom(t => t.From.Any() ? t.From.First().Address : string.Empty))
                .ForMember(d => d.SendDate, opt => opt.MapFrom(t => t.Date))
                .ForAllOtherMembers(d => d.Ignore());
        }
    }
}
