using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using PollaEngendrilClientHosted.Client.State;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Net.NetworkInformation;
using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;

namespace PollaEngendrilClientHosted.Client.Pages
{
    public partial class FixturesComponent
    {

        private IEnumerable<FixtureViewModel> fixtures;

        bool IsInputDisabled(FixtureViewModel fixture, bool isHome)
        {
            if (Env.IsDevelopment())
                return false;

            if (isHome)
            {
                return fixture.HomeTeamRealScore.HasValue || fixture.IsLocked;
            }
            return fixture.AwayTeamRealScore.HasValue || fixture.IsLocked;
        }

        private void ToggleExpansion(FixtureViewModel fixture, bool newValue)
        {
            fixture.ShowOtherPredictions = newValue;
            StateHasChanged();
        }

        private async Task GetUserId()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                if (!UserState.UserId.HasValue)
                {
                    var userName = user.Identity.Name;
                    //userName = "A. Casares";
                    //userName = "Juan Pablo Aliaga";
                    //userName = "Miguel S. Cartagenova M.";
                    //userName = "sebasaliaga2515@gmail.com";
                    //userName = "Rafael Weisson";
                    //userName = "Javier Cañas";
                    var userId = await usersApiService.GetUserIdByUserName(userName);
                    if (userId == -1)
                    {
                        userId = await usersApiService.CreateUser(userName);
                    }
                    UserState.UserId = userId;
                }
            }
        }


        private async Task LoadFixtures()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                if (user?.Identity?.Name != null)
                {
                    fixtures = await fixturesApiService.GetFixtures(user.Identity.Name);
                }
            }
        }

        private bool ValidateScoreInput(FixtureViewModel model, int? score, bool isHome)
        {
            if (!score.HasValue || score < 0)
            {
                if (isHome)
                {
                    model.IsHomeScoreInvalid = false;
                }
                else
                {
                    model.IsAwayScoreInvalid = false;
                }
                return false;
            }

            if (isHome)
            {
                model.IsHomeScoreInvalid = true;
            }
            else
            {
                model.IsAwayScoreInvalid = true;
            }

            return true;
        }

        private async Task ValidateAndSave(FixtureViewModel fixture)
        {
            var isHomeScoreValid = ValidateScoreInput(fixture, fixture.HomeTeamPredictedScore, true);
            var isAwayScoreValid = ValidateScoreInput(fixture, fixture.AwayTeamPredictedScore, false);

            fixture.IsHomeScoreInvalid = !isHomeScoreValid;
            fixture.IsAwayScoreInvalid = !isAwayScoreValid;

            if (isHomeScoreValid && isAwayScoreValid)
            {
                await SavePredictionsAsync(fixture);
            }
        }

        private async Task SavePredictionsAsync(FixtureViewModel model)
        {
            var prediction = new PredictionRequestDTO
            {
                MatchId = model.Id,
                HomeTeamScore = model.HomeTeamPredictedScore,
                AwayTeamScore = model.AwayTeamPredictedScore,
                UserId = UserState?.UserId ?? 0
            };

            if (await predictionApiService.SavePredictionsAsync(prediction))
            {
                model.MustShowSavedSuccesfully = true;
                StateHasChanged();
                await Task.Delay(500);
                model.MustShowSavedSuccesfully = false;
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUserId();
            await LoadFixtures();
        }
    }
}