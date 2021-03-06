﻿/**
* Safe Pad, a double encrypted note pad that uses 2 passwords to protect your documents and help you keep your privacy.
* 
* Copyright (C) 2014 Stephen Haunts
* http://www.stephenhaunts.com
* 
* This file is part of Safe Pad.
* 
* Safe Pad is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Safe Pad is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Authors: Stephen Haunts
*/

using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace HauntedHouseSoftware.SecureNotePad.DomainObjects
{
    public sealed class ApplicationSettings
    {
        public ApplicationSettings()
        {
            RecentFileList = new Collection<string>();
           // NotebookCollection = new NotebookCollection();
        }

        public int WindowPositionX { get; set; }
        public int WindowPositionY { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public FormWindowState FormWindowState { get; set; }
        public byte BackgroundColorRed { get; set; }
        public byte BackgroundColorGreen { get; set; }
        public byte BackgroundColorBlue { get; set; }
        public byte ForegroundColorRed { get; set; }
        public byte ForegroundColorGreen { get; set; }
        public byte ForegroundColorBlue { get; set; }

        public bool DetectUrl { get; set; }

        public bool WordWrap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public Collection<string> RecentFileList { get; set; }
        
        //public NotebookCollection NotebookCollection { get; set; }
    }
}
