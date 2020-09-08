using DotNet.Testcontainers.Containers.Configurations.Abstractions;
using DotNet.Testcontainers.Containers.OutputConsumers;
using DotNet.Testcontainers.Containers.WaitStrategies;
using System;
using System.IO;


namespace PoC.TestesServicos.Tests.Fixtures.Configurations.Databases
{
  public sealed class CouchbaseTestcontainerConfiguration : TestcontainerDatabaseConfiguration
  {
    private const string CouchbaseImage = "mustafaonuraydin/couchbase-testcontainer:6.5.1";
    private const string WaitUntilMessageIsLogged = "couchbase-dev started";
    private const int DefaultClusterRamSize = 1024;
    private const int DefaultClusterIndexRamSize = 512;
    private const int DefaultClusterEventingRamSize = 256;
    private const int DefaultClusterFtsRamSize = 256;
    private const int DefaultClusterAnalyticsRamSize = 1024;
    private const int BootstrapHttpPort = 8091;
    private readonly MemoryStream stdout = new MemoryStream();
    private readonly MemoryStream stderr = new MemoryStream();

    public CouchbaseTestcontainerConfiguration(int defaultPort, int port)
      : base(CouchbaseImage,  defaultPort, port)
    {
      this.OutputConsumer = Consume.RedirectStdoutAndStderrToStream((Stream) this.stderr, (Stream) this.stdout);
      //this.WaitStrategy = Wait.ForUnixContainer().unUntilMessageIsLogged(this.OutputConsumer.Stdout, WaitUntilMessageIsLogged);
      this.WaitStrategy = Wait.ForUnixContainer().UntilPortIsAvailable(8091);
    }

    public string BucketName
    {
      get => this.Environments["BUCKET_NAME"];
      set => this.Environments["BUCKET_NAME"] = value;
    }

    public string BucketType
    {
      get => this.Environments["BUCKET_TYPE"];
      set => this.Environments["BUCKET_TYPE"] = value;
    }

    public string BucketRamSize
    {
      get => this.Environments["BUCKET_RAMSIZE"];
      set => this.Environments["BUCKET_RAMSIZE"] = value;
    }

    public string ClusterRamSize
    {
      get => this.Environments["CLUSTER_RAMSIZE"];
      set
      {
        CouchbaseTestcontainerConfiguration.ThrowIfMemoryIsLessThanMinimum(nameof (ClusterRamSize), value, 1024);
        this.Environments["CLUSTER_RAMSIZE"] = value;
      }
    }

    public string ClusterIndexRamSize
    {
      get => this.Environments["CLUSTER_INDEX_RAMSIZE"];
      set
      {
        CouchbaseTestcontainerConfiguration.ThrowIfMemoryIsLessThanMinimum(nameof (ClusterIndexRamSize), value, 512);
        this.Environments["CLUSTER_INDEX_RAMSIZE"] = value;
      }
    }

    public string ClusterEventingRamSize
    {
      get => this.Environments["CLUSTER_EVENTING_RAMSIZE"];
      set
      {
        CouchbaseTestcontainerConfiguration.ThrowIfMemoryIsLessThanMinimum(nameof (ClusterEventingRamSize), value, 256);
        this.Environments["CLUSTER_EVENTING_RAMSIZE"] = value;
      }
    }

    public string ClusterFtsRamSize
    {
      get => this.Environments["CLUSTER_FTS_RAMSIZE"];
      set
      {
        CouchbaseTestcontainerConfiguration.ThrowIfMemoryIsLessThanMinimum(nameof (ClusterFtsRamSize), value, 256);
        this.Environments["CLUSTER_FTS_RAMSIZE"] = value;
      }
    }

    public string ClusterAnalyticsRamSize
    {
      get => this.Environments["CLUSTER_ANALYTICS_RAMSIZE"];
      set
      {
        CouchbaseTestcontainerConfiguration.ThrowIfMemoryIsLessThanMinimum(nameof (ClusterAnalyticsRamSize), value, 1024);
        this.Environments["CLUSTER_ANALYTICS_RAMSIZE"] = value;
      }
    }

    public override string Username
    {
      get => this.Environments["USERNAME"];
      set => this.Environments["USERNAME"] = value;
    }

    public override string Password
    {
      get => this.Environments["PASSWORD"];
      set => this.Environments["PASSWORD"] = value;
    }

    public override IOutputConsumer OutputConsumer { get; }

    public override IWaitForContainerOS WaitStrategy { get; }

    private static void ThrowIfMemoryIsLessThanMinimum(
      string propertyName,
      string value,
      int minimumMemoryInMb)
    {
      int result;
      if (!int.TryParse(value, out result))
        throw new ArgumentException(value + " is not an integer.", propertyName);
      if (result < minimumMemoryInMb)
        throw new ArgumentOutOfRangeException(propertyName, string.Format("Couchbase {0} ram size can not be less than {1} MB.", (object) propertyName, (object) minimumMemoryInMb));
    }
  } 
}