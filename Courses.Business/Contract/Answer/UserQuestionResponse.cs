﻿namespace Courses.Business.Contract.Answer;

public record UserQuestionResponse(
    int Id,
    string Text,
    bool Correct,
    int SelecedOptionId,
    IList<UserOptionResponse> Options
    );