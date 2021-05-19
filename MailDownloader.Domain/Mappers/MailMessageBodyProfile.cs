using AutoMapper;
using MailDownloader.Domain.Models;
using Limilabs.Mail;

namespace MailDownloader.Domain.Mappers
{
    internal class MailMessageBodyProfile : Profile
    {
        public MailMessageBodyProfile()
        {
            CreateMap<IMail, MailMessageBody>()
                .ForMember(d => d.BodyHtml, opt => opt.MapFrom(t => t.Html))
                .ForMember(d => d.BodyText, opt => opt.MapFrom(t => t.Text))
                .ForAllOtherMembers(d => d.Ignore());
        }
    }
}
