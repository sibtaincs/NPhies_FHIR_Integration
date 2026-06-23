using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NPhies_FHIR_Integration.Application.Services.RCM;

/// <summary>
/// Performance Optimization Service Interface
/// Manages caching, query optimization, and performance monitoring
/// </summary>
public interface IPerformanceOptimizationService
{
    /// <summary>
    /// Initialize caching strategy
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Initialization result</returns>
    Task<PerformanceInitializationResult> InitializeCachingAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get or cache dashboard metrics
    /// </summary>
    /// <param name="cacheKey">Cache key</param>
    /// <param name="cacheDurationMinutes">Cache duration</param>
/// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Cached metrics</returns>
    Task<CachedMetricsResult> GetCachedMetricsAsync(
        string cacheKey,
 int cacheDurationMinutes = 5,
        CancellationToken cancellationToken = default);

    /// <summary>
 /// Optimize database queries
    /// </summary>
    /// <param name="queryType">Type of query to optimize</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Optimization result</returns>
    Task<QueryOptimizationResult> OptimizeQueryAsync(
        string queryType,
        CancellationToken cancellationToken = default);

 /// <summary>
    /// Monitor performance metrics
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Performance metrics</returns>
Task<PerformanceMetricsSnapshot> MonitorPerformanceAsync(
CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate performance report
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Performance report</returns>
    Task<PerformanceReport> GeneratePerformanceReportAsync(
 DateTime fromDate,
      DateTime toDate,
 CancellationToken cancellationToken = default);

    /// <summary>
/// Clear cache
    /// </summary>
    /// <param name="cacheKey">Optional cache key to clear specific cache</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Clear result</returns>
    Task<bool> ClearCacheAsync(
        string cacheKey = null,
  CancellationToken cancellationToken = default);
}

/// <summary>
/// Performance Initialization Result
/// </summary>
public class PerformanceInitializationResult
{
    /// <summary>
    /// Whether initialization was successful
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Number of caching strategies configured
    /// </summary>
    public int CachingStrategiesConfigured { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Cache strategies enabled
    /// </summary>
    public List<string> StrategiesEnabled { get; set; } = new();
}

/// <summary>
/// Cached Metrics Result
/// </summary>
public class CachedMetricsResult
{
    /// <summary>
    /// Whether from cache
    /// </summary>
    public bool FromCache { get; set; }

    /// <summary>
    /// Cache key used
    /// </summary>
    public string CacheKey { get; set; } = string.Empty;

    /// <summary>
    /// Data
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();

    /// <summary>
    /// Retrieval time (milliseconds)
  /// </summary>
    public long RetrievalTimeMs { get; set; }

/// <summary>
    /// Cache expiration time
    /// </summary>
    public DateTime ExpirationTime { get; set; }
}

/// <summary>
/// Query Optimization Result
/// </summary>
public class QueryOptimizationResult
{
    /// <summary>
    /// Query type
    /// </summary>
    public string QueryType { get; set; } = string.Empty;

    /// <summary>
    /// Original execution time (ms)
 /// </summary>
    public long OriginalExecutionTimeMs { get; set; }

    /// <summary>
    /// Optimized execution time (ms)
    /// </summary>
    public long OptimizedExecutionTimeMs { get; set; }

    /// <summary>
  /// Performance improvement percentage
    /// </summary>
 public decimal ImprovementPercentage { get; set; }

  /// <summary>
    /// Optimization recommendations
    /// </summary>
    public List<string> Recommendations { get; set; } = new();

    /// <summary>
    /// Query optimization applied
    /// </summary>
    public string OptimizationApplied { get; set; } = string.Empty;
}

/// <summary>
/// Performance Metrics Snapshot
/// </summary>
public class PerformanceMetricsSnapshot
{
    /// <summary>
    /// Snapshot time
    /// </summary>
    public DateTime SnapshotTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Average response time (ms)
    /// </summary>
    public double AverageResponseTimeMs { get; set; }

    /// <summary>
    /// P95 response time (ms)
    /// </summary>
    public double P95ResponseTimeMs { get; set; }

    /// <summary>
    /// P99 response time (ms)
    /// </summary>
    public double P99ResponseTimeMs { get; set; }

 /// <summary>
    /// Throughput (requests/second)
    /// </summary>
    public double ThroughputRps { get; set; }

  /// <summary>
    /// Cache hit rate percentage
    /// </summary>
    public decimal CacheHitRatePercentage { get; set; }

    /// <summary>
  /// Error rate percentage
    /// </summary>
    public decimal ErrorRatePercentage { get; set; }

    /// <summary>
    /// Memory usage (MB)
    /// </summary>
    public long MemoryUsageMb { get; set; }

    /// <summary>
    /// CPU usage percentage
    /// </summary>
    public decimal CpuUsagePercentage { get; set; }
}

/// <summary>
/// Performance Report
/// </summary>
public class PerformanceReport
{
    /// <summary>
    /// Report ID
    /// </summary>
    public string ReportId { get; set; } = string.Empty;

    /// <summary>
    /// Report date
    /// </summary>
    public DateTime ReportDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Reporting period
    /// </summary>
    public string ReportingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Average response time
    /// </summary>
    public double AverageResponseTimeMs { get; set; }

    /// <summary>
    /// Peak response time
    /// </summary>
    public double PeakResponseTimeMs { get; set; }

 /// <summary>
    /// Cache hit rate
    /// </summary>
    public decimal CacheHitRatePercentage { get; set; }

    /// <summary>
    /// Query performance breakdown
    /// </summary>
    public Dictionary<string, double> QueryPerformanceBreakdown { get; set; } = new();

    /// <summary>
    /// Performance bottlenecks identified
    /// </summary>
    public List<string> BottlenecksIdentified { get; set; } = new();

    /// <summary>
    /// Optimization recommendations
    /// </summary>
    public List<string> OptimizationRecommendations { get; set; } = new();

    /// <summary>
    /// Overall performance score (0-100)
    /// </summary>
    public decimal PerformanceScore { get; set; }
}
