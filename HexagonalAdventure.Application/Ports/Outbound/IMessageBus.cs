namespace HexagonalAdventure.Application.Ports.Outbound;

/*
    Dış dünyaya göndereceğimiz mesajlar için kullanılacak genel bir outbound port arayüzü.
    Örneğin entegrasyon olaylarını ele alalım.
    Ürün stok hareketlerindeki değişiklikler gibi. Bunları ele alacak dış sistemler için tasarlanmış bir
    arayüz olarak düşünebiliriz. 
    Bu sayede RabbitMq, Kafka, Redis gibi mesajlaşma altyapılarını uygulama katmanımızdan soyutlamış oluruz. 
*/
public interface IMessageBus
{
    Task PublishAsync<TMessage>(TMessage message, string topicName);
}
