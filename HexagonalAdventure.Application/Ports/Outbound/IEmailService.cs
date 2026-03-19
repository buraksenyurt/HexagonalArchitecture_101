namespace HexagonalAdventure.Application.Ports.Outbound;

/*
 Bir başka dış servis entegrasyonu örneği.
 Sistemden email gönderilmesi gereken durumlar için bir out port sözleşmesi tanımladık.
 Bunun somut örneği aslında bir Outbound Adapter projesidir.
 HexagonalAdventure.Adapters.Out.Email projesine bakın.
 */
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
