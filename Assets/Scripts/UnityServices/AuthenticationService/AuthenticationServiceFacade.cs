using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Abstracts;

public class AuthenticationServiceFacade : BaseAuthenticationServiceFacade
{
    private readonly BaseProfileManager _profileManager;

    public AuthenticationServiceFacade()
    {
        _profileManager = ServiceLocator.Instance.GetService<ProfileManagerPlayerPrefs>(true);
    }

    public override async Task<bool> TryAuthorizePlayerAsync()
    {
        try
        {
            await TryInitializeUnityServicesAsync();
            if (_profileManager.CurrentProfileName != AuthenticationService.Instance.Profile) SwitchProfile(_profileManager.CurrentProfileName);
            if (AuthenticationService.Instance.IsAuthorized) return true;
            await TrySignInAsync();
            return true;
        }
        catch (ServicesInitializationException)
        {
            // POPUP
            Debug.LogError("Error while trying to initialize UnityServices");
            return false;
        }
        catch (AuthenticationException)
        {
            // POPUP
            Debug.LogError("Error while trying to sign in to AuthenticationService!");
            return false;
        }
    }

    private async Task TryInitializeUnityServicesAsync()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (ServicesInitializationException)
            {
                throw;
            }
        }
    }

    private async Task TrySignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Signed in as { AuthenticationService.Instance.Profile }");
        }
        catch (ServicesInitializationException)
        {
            throw;
        }
        catch (AuthenticationException)
        {
            throw;
        }
    }

    private void SwitchProfile(string profileName)
    {
        if (AuthenticationService.Instance.IsSignedIn) AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.SwitchProfile(profileName);
    }
}
