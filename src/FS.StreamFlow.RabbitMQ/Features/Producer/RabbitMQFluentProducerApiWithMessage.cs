using FS.StreamFlow.Core.Features.Messaging.Interfaces;
using FS.StreamFlow.Core.Features.Messaging.Models;

namespace FS.StreamFlow.RabbitMQ.Features.Producer;

/// <summary>
/// RabbitMQ implementation of fluent producer API with pre-configured message
/// </summary>
/// <typeparam name="T">Message type</typeparam>
public class RabbitMQFluentProducerApiWithMessage<T> : IFluentProducerApi<T> where T : class
{
    private readonly IProducer _producer;
    private readonly T _message;
    private readonly MessageProperties _properties;
    private string _exchange = string.Empty;
    private string _routingKey = string.Empty;
    private bool _mandatory;
    private TimeSpan _confirmationTimeout = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Initializes a new instance of the RabbitMQ fluent producer API with message
    /// </summary>
    /// <param name="producer">Producer instance</param>
    /// <param name="message">Pre-configured message</param>
    public RabbitMQFluentProducerApiWithMessage(IProducer producer, T message)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _message = message ?? throw new ArgumentNullException(nameof(message));
        _properties = new MessageProperties();
    }

    /// <summary>
    /// Configures the exchange to publish to
    /// </summary>
    /// <param name="exchangeName">Exchange name</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithExchange(string exchangeName)
    {
        _exchange = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
        return this;
    }

    /// <summary>
    /// Configures the exchange to publish to (alternative syntax)
    /// </summary>
    /// <param name="exchangeName">Exchange name</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> ToExchange(string exchangeName)
    {
        return WithExchange(exchangeName);
    }

    /// <summary>
    /// Configures the routing key for message routing
    /// </summary>
    /// <param name="routingKey">Routing key</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithRoutingKey(string routingKey)
    {
        _routingKey = routingKey ?? throw new ArgumentNullException(nameof(routingKey));
        return this;
    }

    /// <summary>
    /// Configures message delivery mode
    /// </summary>
    /// <param name="deliveryMode">Delivery mode (persistent or non-persistent)</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithDeliveryMode(DeliveryMode deliveryMode)
    {
        _properties.DeliveryMode = deliveryMode;
        return this;
    }

    /// <summary>
    /// Configures message priority
    /// </summary>
    /// <param name="priority">Message priority (0-255)</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithPriority(byte priority)
    {
        _properties.Priority = priority;
        return this;
    }

    /// <summary>
    /// Configures message expiration
    /// </summary>
    /// <param name="expiration">Message expiration time</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithExpiration(TimeSpan expiration)
    {
        _properties.Expiration = expiration.TotalMilliseconds.ToString();
        return this;
    }

    /// <summary>
    /// Configures message headers
    /// </summary>
    /// <param name="headers">Message headers dictionary</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithHeaders(Dictionary<string, object> headers)
    {
        _properties.Headers = headers ?? throw new ArgumentNullException(nameof(headers));
        return this;
    }

    /// <summary>
    /// Adds a single header to the message
    /// </summary>
    /// <param name="key">Header key</param>
    /// <param name="value">Header value</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithHeader(string key, object value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key));

        _properties.Headers ??= new Dictionary<string, object>();
        _properties.Headers[key] = value;
        return this;
    }

    /// <summary>
    /// Configures content type
    /// </summary>
    /// <param name="contentType">Content type (e.g., "application/json")</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithContentType(string contentType)
    {
        _properties.ContentType = contentType;
        return this;
    }

    /// <summary>
    /// Configures content encoding
    /// </summary>
    /// <param name="contentEncoding">Content encoding (e.g., "utf-8")</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithContentEncoding(string contentEncoding)
    {
        _properties.ContentEncoding = contentEncoding;
        return this;
    }

    /// <summary>
    /// Configures correlation ID for message tracking
    /// </summary>
    /// <param name="correlationId">Correlation ID</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithCorrelationId(string correlationId)
    {
        _properties.CorrelationId = correlationId;
        return this;
    }

    /// <summary>
    /// Configures message ID
    /// </summary>
    /// <param name="messageId">Message ID</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithMessageId(string messageId)
    {
        _properties.MessageId = messageId;
        return this;
    }

    /// <summary>
    /// Configures whether the message is mandatory
    /// </summary>
    /// <param name="mandatory">Mandatory flag</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithMandatory(bool mandatory = true)
    {
        _mandatory = mandatory;
        return this;
    }

    /// <summary>
    /// Configures publisher confirmation timeout
    /// </summary>
    /// <param name="timeout">Confirmation timeout</param>
    /// <returns>Fluent producer API for method chaining</returns>
    public IFluentProducerApi<T> WithConfirmationTimeout(TimeSpan timeout)
    {
        _confirmationTimeout = timeout;
        return this;
    }

    /// <summary>
    /// Publishes the pre-configured message with the configured settings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the publish operation with results</returns>
    public async Task<PublishResult> PublishAsync(CancellationToken cancellationToken = default)
    {
        return await PublishAsync(_message, cancellationToken);
    }

    /// <summary>
    /// Publishes the message with the configured settings
    /// </summary>
    /// <param name="message">Message to publish (overrides pre-configured message)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the publish operation with results</returns>
    public async Task<PublishResult> PublishAsync(T message, CancellationToken cancellationToken = default)
    {
        if (message == null)
            throw new ArgumentNullException(nameof(message));

        if (string.IsNullOrEmpty(_exchange))
            throw new InvalidOperationException("Exchange must be configured before publishing");

        // Cast to RabbitMQProducer to access the PublishAsync method with MessageProperties
        if (_producer is RabbitMQProducer rabbitProducer)
        {
            return await rabbitProducer.PublishAsync(_exchange, _routingKey, message, _properties, cancellationToken);
        }

        // Fallback to interface method
        var success = await _producer.PublishAsync(_exchange, _routingKey, message, cancellationToken: cancellationToken);
        return new PublishResult
        {
            IsSuccess = success,
            Exchange = _exchange,
            RoutingKey = _routingKey,
            MessageId = _properties.MessageId ?? Guid.CreateVersion7().ToString(),
            CorrelationId = _properties.CorrelationId,
            Timestamp = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Publishes the pre-configured message synchronously with the configured settings
    /// </summary>
    /// <returns>Publish result</returns>
    public PublishResult Publish()
    {
        return Publish(_message);
    }

    /// <summary>
    /// Publishes the message synchronously with the configured settings
    /// </summary>
    /// <param name="message">Message to publish (overrides pre-configured message)</param>
    /// <returns>Publish result</returns>
    public PublishResult Publish(T message)
    {
        return PublishAsync(message).GetAwaiter().GetResult();
    }
} 