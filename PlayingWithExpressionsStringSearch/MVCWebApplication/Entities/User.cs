﻿using System;
using System.Collections.Generic;

namespace MVCWebApplication.Entities;

public partial class User
{
    public int IdUser { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}