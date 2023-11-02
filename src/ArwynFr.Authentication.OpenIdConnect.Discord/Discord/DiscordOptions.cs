﻿using System.ComponentModel.DataAnnotations;

namespace ArwynFr.Authentication.OpenIdConnect.Discord.Discord;

public record DiscordOptions
{
    public const string ConfigurationPath = "Discord";

    public PathString CallbackPath { get; set; } = new PathString(DiscordDefaults.DefaultCallbackPath);

    [Required]
    public string ClientId { get; set; } = string.Empty;

    [Required]
    public string ClientSecret { get; set; } = string.Empty;
}
