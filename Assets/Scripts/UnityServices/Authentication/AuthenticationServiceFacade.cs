using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using System;
using Abstracts;

public class AuthenticationServiceFacade : BaseAuthenticationServiceFacade
{
    private BaseProfileManager _profileManager;

    public AuthenticationServiceFacade()
    {
        _profileManager = ServiceLocator.Instance.GetService<ProfileManagerPlayerPrefs>(true);
    }

    public override async Task<bool> TryAuthorizePlayerAsync()
    {
        try
        {
            await TryInitializeUnityServicesAsync();
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
        catch (RequestFailedException)
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
            if (AuthenticationService.Instance.IsSignedIn) AuthenticationService.Instance.SignOut();
            if (_profileManager.CurrentProfileName != AuthenticationService.Instance.Profile) AuthenticationService.Instance.SwitchProfile(_profileManager.CurrentProfileName);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Signed in as { AuthenticationService.Instance.Profile }");
        }
        catch (ServicesInitializationException)
        {
            throw;
        }
        catch (RequestFailedException)
        {
            throw;
        }
    }
}
