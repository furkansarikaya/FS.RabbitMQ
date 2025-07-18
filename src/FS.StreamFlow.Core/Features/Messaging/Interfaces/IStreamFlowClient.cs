using FS.StreamFlow.Core.Features.Messaging.Models;
using FS.StreamFlow.Core.Features.Events.Interfaces;

namespace FS.StreamFlow.Core.Features.Messaging.Interfaces;

/// <summary>
/// Main StreamFlow client interface providing high-level operations for messaging, event-driven architecture, and connection management
/// </summary>
public interface IStreamFlowClient : IDisposable
{
    /// <summary>
    /// Gets connection manager for managing messaging connections with auto-reconnection and health monitoring
    /// </summary>
    IConnectionManager ConnectionManager { get; }
    
    /// <summary>
    /// Gets exchange manager for exchange declaration, binding, and management operations
    /// </summary>
    IExchangeManager ExchangeManager { get; }
    
    /// <summary>
    /// Gets queue manager for queue declaration, binding, and management operations
    /// </summary>
    IQueueManager QueueManager { get; }
    
    /// <summary>
    /// Gets message producer for publishing messages with retry policies and error handling
    /// </summary>
    IProducer Producer { get; }
    
    /// <summary>
    /// Gets message consumer for consuming messages with automatic acknowledgment and error handling
    /// </summary>
    IConsumer Consumer { get; }
    
    /// <summary>
    /// Gets event bus for event-driven messaging, domain events, and integration events
    /// </summary>
    IEventBus EventBus { get; }
    
    /// <summary>
    /// Gets event store for event sourcing operations and aggregate reconstruction
    /// </summary>
    IEventStore EventStore { get; }
    
    /// <summary>
    /// Gets health checker for monitoring connection health and service availability
    /// </summary>
    IHealthChecker HealthChecker { get; }
    
    /// <summary>
    /// Gets saga orchestrator for managing long-running workflow processes and compensation patterns
    /// </summary>
    ISagaOrchestrator SagaOrchestrator { get; }
    
    /// <summary>
    /// Gets retry policy factory for creating and managing retry strategies
    /// </summary>
    IRetryPolicyFactory RetryPolicyFactory { get; }
    
    /// <summary>
    /// Gets message serializer factory for creating serializers for different formats
    /// </summary>
    IMessageSerializerFactory SerializerFactory { get; }
    
    /// <summary>
    /// Gets error handler for comprehensive error management and dead letter queue support
    /// </summary>
    IErrorHandler ErrorHandler { get; }
    
    /// <summary>
    /// Gets dead letter handler for managing failed messages and reprocessing
    /// </summary>
    IDeadLetterHandler DeadLetterHandler { get; }
    
    /// <summary>
    /// Gets metrics collector for monitoring and observability
    /// </summary>
    IMetricsCollector MetricsCollector { get; }
    
    /// <summary>
    /// Gets the current client status and connection state
    /// </summary>
    ClientStatus Status { get; }
    
    /// <summary>
    /// Initializes the client and establishes initial connections
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the initialization operation</returns>
    Task InitializeAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Shuts down the client gracefully, closing all connections and releasing resources
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the shutdown operation</returns>
    Task ShutdownAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Event raised when client status changes
    /// </summary>
    event EventHandler<ClientStatusChangedEventArgs>? StatusChanged;
}