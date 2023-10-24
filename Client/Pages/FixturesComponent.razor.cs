using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using PollaEngendrilClientHosted.Client.State;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;
using System.Net.NetworkInformation;
using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace PollaEngendrilClientHosted.Client.Pages
{
    public partial class FixturesComponent
    {

        private IEnumerable<FixtureViewModel> fixtures;
        private int TotalPointsObtained;
        private List<UserPredictionViewModel> allPredictions;
        private bool isFirstExecution = true;
        private bool hideOldMatches = true;
        private IJSObjectReference JSmoduleScroll;
        bool IsInputDisabled(FixtureViewModel fixture)
        {
            //if (Env.IsDevelopment())
            //    return false;

            return fixture.IsLocked;
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
                    TotalPointsObtained = fixtures.Sum(f => f.PointsObtained ?? 0);
                }
            }
        }

        private async Task LoadPredictions()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                if (user?.Identity?.Name != null)
                {
                    allPredictions = await predictionApiService.GetAllPredictionsAsync();
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (isFirstExecution)
            {
                if (fixtures != null)
                {
                    foreach (var fixture in fixtures)
                    {
                        if (!fixture.HomeTeamRealScore.HasValue)
                        {
                            await JSmoduleScroll.InvokeVoidAsync("scrollToElement", $"homeScore{fixture.Id}");
                            isFirstExecution = false;
                            break;
                        }
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
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
            JSmoduleScroll = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/FixturesComponent.razor.js");
            await GetUserId();
            await LoadPredictions();
            await LoadFixtures();
        }
    }
}