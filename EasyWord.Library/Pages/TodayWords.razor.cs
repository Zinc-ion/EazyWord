﻿using BootstrapBlazor.Components;
using EasyWord.Library.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using EasyWord.Library.Services;

namespace EasyWord.Library.Pages;

public partial class TodayWords
{
    private List<Word> _words = new();

    private int pageSize = 5;

    private Expression<Func<Word, bool>> _where = p => p.Status == 0;

    private bool soLittleWords = false;

    private string _wordsToReading = "";
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();
    }


    private async void IncrementPageSize()
    {
        ++pageSize;
        if (pageSize > 20)
        {
            soLittleWords = true;
            --pageSize;
            await ToastService.Error("不能再加了", "背这么多要复习不完喽", autoHide: true);

        }
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }
    private async void DecrementPageSize()
    {
        --pageSize;
        if (pageSize < 1)
        {
            soLittleWords = true;
            ++pageSize;
            await ToastService.Error("不能再减了", "至少要背一个吧", autoHide: true);

        }
        StateHasChanged();
        var words = await _wordStorage.GetWordsAsync(_where, 0, pageSize);
        _words.Clear();
        _words.AddRange(words);
        StateHasChanged();

    }

    //认识
    public async Task<int> KnowWord(int wordRank)
    {
        await _wordStorage.KnowWord(wordRank);

        return 1;
    }

    //不认识单词
    public async Task<int> UnknownWord(int wordRank)
    {
        await _wordStorage.UnknownWord(wordRank);

        return 1;
    }

    private async void GenerateReading()
    {
        foreach (Word word in _words)
        {
            _wordsToReading = _wordsToReading + " " + word.HeadWord;
        }
        _navigationService.NavigateTo("/reading/_wordsToReading");
    }

    private void GoToDetail(int wordRank)
    {
        _navigationService.NavigateTo("/wordDetail/wordRank");
    }


    private void OnClick(Word word) =>
        _navigationService.NavigateTo(
            $"{NavigationServiceConstants.WordDetail}/{word.WordRank}");
}