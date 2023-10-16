#define ALLOW_ENTER_ALL_RESULTS
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using PollaEngendrilClientHosted.Client.State;
using PollaEngendrilClientHosted.Shared.Models.DTO;
using PollaEngendrilClientHosted.Shared.Models.ViewModel;

namespace PollaEngendrilClientHosted.Client.Pages
{
    public partial class FixturesComponent
    {

        private IEnumerable<FixtureViewModel> fixtures;

        bool IsInputDisabled(FixtureViewModel fixture, bool isHome)
        {
            #if ALLOW_ENTER_ALL_RESULTS
            return false;
            #endif

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
            if (!score.HasValue || !int.TryParse(score.Value.ToString(), out int _result ))
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