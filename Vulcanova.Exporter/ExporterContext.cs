using System.Security.Cryptography.X509Certificates;
using Vulcanova.Uonet;
using Vulcanova.Uonet.Api;
using Vulcanova.Uonet.Api.Auth;
using Vulcanova.Uonet.Api.Common;
using Vulcanova.Uonet.Crypto;
using Vulcanova.Uonet.Firebase;
using Vulcanova.Uonet.Signing;

namespace Vulcanova.Exporter;

public class ExporterContext
{
    public AccountPayload Account { get; }
    public IApiClient ApiClient { get; }

    private ExporterContext(AccountPayload account, IApiClient apiClient)
    {
        Account = account;
        ApiClient = apiClient;
    }

    public static async Task<ExporterContext> Create(string token, string symbol, string pin)
    {
        var firebaseToken = await FirebaseTokenFetcher.FetchFirebaseTokenAsync();
        var (pk, cert) = KeyPairGenerator.GenerateKeyPair();

        var x509Cert2 = new X509Certificate2(cert.GetEncoded());

        var requestSigner = new RequestSigner(x509Cert2.Thumbprint, pk, firebaseToken);

        var instanceUrlProvider = new InstanceUrlProvider();

        var apiClient = new ApiClient(requestSigner, await instanceUrlProvider.GetInstanceUrlAsync(token, symbol));

        var request = new RegisterClientRequest
        {
            OS = Constants.AppOs,
            DeviceModel = Constants.DeviceModel,
            Certificate = Convert.ToBase64String(x509Cert2.RawData),
            CertificateType = "X509",
            CertificateThumbprint = x509Cert2.Thumbprint,
            PIN = pin,
            SecurityToken = token,
            SelfIdentifier = Guid.NewGuid().ToString()
        };

        await apiClient.PostAsync(RegisterClientRequest.ApiEndpoint, request);

        var registerHebeResponse = await apiClient.GetAsync(RegisterHebeClientQuery.ApiEndpoint, new RegisterHebeClientQuery());

        var firstAccount = registerHebeResponse.Envelope[0];

        var contextualSigner = new ContextualRequestSigner(x509Cert2.Thumbprint, pk, firebaseToken, firstAccount.Context);

        var unitApiClient = new ApiClient(contextualSigner, firstAccount.Unit.RestUrl.ToString());

        return new ExporterContext(firstAccount, unitApiClient);
    }
}