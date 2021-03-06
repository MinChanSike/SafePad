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
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Diagnostics;
using System.IO;
using HauntedHouseSoftware.SecureNotePad.Forms.PasswordGenerator;

namespace HauntedHouseSoftware.SecureNotePad.Forms
{
    public partial class MainForm : Form
    {
        private FindAndReplaceForm _findForm;
        private const string HelpUrl = "http://stephenhaunts.com/projects/safe-pad/safe-pad-1-3-manual/";

        public MainForm(string fileName)
        {
            InitializeComponent();
            Text = $"{ApplicationName} : {_documentName}";
            Visible = true;
            richTextBox.Visible = true;
            PopulateFontDropDown();
            _findForm = new FindAndReplaceForm(richTextBox);
            LoadSettings();           
            LoadDocument(fileName);
            ChangeDisplayHeader();
            ResizeRedraw = true;
            Invalidate(true);
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public MainForm()
        {
            InitializeComponent();
            PopulateFontDropDown();
            UpdateFontDropDownWithFontSelection();
            ChangeDisplayHeader();
            _findForm = new FindAndReplaceForm(richTextBox);
            LoadSettings();
            ResizeRedraw = true;
            richTextBox.Width = richTextBox.Width + 1;                        
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            ExitApplication();
        }


        private void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            ExitApplication();            
        }
        
        private void SaveToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void LoadToolStripMenuItemClick(object sender, EventArgs e)
        {
            LoadDocument();            
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            ToolStripManager.LoadSettings(this, "SecureNotePad");
            richTextBox.ShowSelectionMargin = true;
            
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
        }

        private void PrintDocumentBeginPrint(object sender, PrintEventArgs e)
        {
            _charFrom = 0;
        }

        private void PrintToolStripMenuItemClick(object sender, EventArgs e)
        {
           printPreviewDialog.ShowDialog();
        }

        private void PrintToolStripMenuItem1Click(object sender, EventArgs e)
        {
            printDialog.ShowDialog();
        }

        private void NewDocumentButtonClick(object sender, EventArgs e)
        {
            if (NewDocument(true) == false)
            {
                return;
            }

            _documentName = string.Empty;
            ChangeDisplayHeader();
            richTextBox.Clear();
        }

        private void ToolStripButton1Click(object sender, EventArgs e)
        {
            LoadDocument();
        }

        private void SaveDocumentButtonClick(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void SelectFontToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox.SelectionFont = fontDialog.Font;
            }
        }

        private void SelectColorToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox.SelectionColor = colorDialog.Color;
            }
        }

        private void PrintDocumentButtonClick(object sender, EventArgs e)
        {
            printPreviewDialog.ShowDialog();
        }

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            richTextBox.Cut();
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            richTextBox.Paste();
        }

        private void CutClick(object sender, EventArgs e)
        {
            richTextBox.Cut();
        }

        private void CopyButtonClick(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void PasteButtonClick(object sender, EventArgs e)
        {
            richTextBox.Paste();            
        }

        private void WordWrapToolStripMenuItemClick(object sender, EventArgs e)
        {
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;
            richTextBox.WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void UndoToolStripMenuItemClick(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }

        private void RedoToolStripMenuItemClick(object sender, EventArgs e)
        {
            richTextBox.Redo();
        }

        private void UndoButtonClick(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }

        private void RedoButtonClick(object sender, EventArgs e)
        {
            richTextBox.Redo();                            
        }

        private void BoldButtonClick(object sender, EventArgs e)
        {
            SetBold();
        }

        private void ItalicButtonClick(object sender, EventArgs e)
        {
            SetItalic();
        }

        private void UnderlineButtonClick(object sender, EventArgs e)
        {
            SetUnderline();
        }

        private void BoldToolStripMenuItemClick(object sender, EventArgs e)
        {
            SetBold();
        }

        private void ItalicToolStripMenuItemClick(object sender, EventArgs e)
        {
            SetItalic();
        }

        private void UnderlineToolStripMenuItemClick(object sender, EventArgs e)
        {
            SetUnderline();
        } 

        private void SaveToolbarLayoutToolStripMenuItemClick(object sender, EventArgs e)
        {
            ToolStripManager.SaveSettings(this, "SecureNotePad");
        }

        private void LoadToolbarLayoutToolStripMenuItemClick(object sender, EventArgs e)
        {
            ToolStripManager.LoadSettings(this, "SecureNotePad");            
        }

        private void ToolStripButton1Click1(object sender, EventArgs e)
        {
            ToggleBulletSelection();
        }


        private void BulletSelectionToolStripMenuItemClick(object sender, EventArgs e)
        {
            ToggleBulletSelection();
        }
        
        private void DecreaseIndentButtonClick(object sender, EventArgs e)
        {
            DecreaseIndent();
        }

        private void IncreaseIndentButtonClick(object sender, EventArgs e)
        {
            IncreaseIndent();
        }      
        
        private void IncreaseIndentToolStripMenuItemClick(object sender, EventArgs e)
        {
            IncreaseIndent();
        }

        private void DecreaseIndentToolStripMenuItemClick(object sender, EventArgs e)
        {
            DecreaseIndent();
        }

        private void LeftJustifyButtonClick(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Left);
        }

        private void CentreButtonClick(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Center);
        }

        private void RightJustifyButtonClick(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Right);
        }

        private void LeftJustifyToolStripMenuItemClick(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Left);
        }

        private void CentreJustifyToolStripMenuItemClick(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Center);
        }

        private void RightJustifyToolStripMenuItemClick(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Right);
        }

        private void RichTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            Text = $"{ApplicationName} : *{_documentName}";

            if (e.KeyChar == (char)Keys.Escape)
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void RichTextBoxLinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                Process.Start(e.LinkText);
            }
            catch
            {
                MessageBox.Show(@"There was an loading the specified link.", @"Error loading Link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertImageToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (insertImageDialog.ShowDialog() != DialogResult.OK) return;

            Clipboard.SetImage(Image.FromFile(insertImageDialog.FileName));
            richTextBox.Paste();
        }

        private void RichTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            UpdateFontStyleButtons();
            UpdateFontDropDownWithFontSelection();

            _documentChanged = true;
        }

        private void NewDocumentToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (NewDocument(true) == false)
            {
                return;
            }

            ChangeDisplayHeader();
            _documentName = string.Empty;
            richTextBox.Clear();
        }

        private void NewDocumentWindowToolStripMenuItemClick(object sender, EventArgs e)
        {
            NewDocumentWindow();
        }
      
        private void NewDocumentWindowToolStripMenuItem1Click(object sender, EventArgs e)
        {
            NewDocumentWindow();
        }

        private void MainFormActivated(object sender, EventArgs e)
        {
            EnableNotifyIcon(true);
        }

        private void AboutToolStripMenuItem1Click(object sender, EventArgs e)
        {
            AboutBox();
        }

        private void RichTextBoxClick(object sender, EventArgs e)
        {
            UpdateFontStyleButtons();
            UpdateFontDropDownWithFontSelection();
        }

        private void toolStripFontSelector_DropDown(object sender, EventArgs e)
        {
            PopulateFontDropDown();
        }

        private void toolStripFontSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFontInRichTextBox();
        }

        private void toolStripFontSizeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFontInRichTextBox();
        }

        private void toolStripFontSizeSelector_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateFontInRichTextBox();
        }

        private void toolStripFontSizeSelector_TextChanged(object sender, EventArgs e)
        {
            UpdateFontInRichTextBox();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDocumentAs();
        }

 
        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.BackColor = Color.White;
            richTextBox.ForeColor = Color.Black;
        }

        private void lowContrastDarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.BackColor = Color.DimGray;
            richTextBox.ForeColor = Color.Gray;
        }

        private void lowContrastLightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.BackColor = Color.Gainsboro;
            richTextBox.ForeColor = Color.Silver;
        }

        private void customBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (themeColorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox.BackColor = themeColorDialog.Color;
            }
        }

        private void customeForegroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (themeColorDialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox.ForeColor = themeColorDialog.Color;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 1;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 2;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 3;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 4;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 5;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 6;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 7;
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 8;
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 9;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 10;
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPropertiesDialog();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void propertiesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenPropertiesDialog();
        }

        private void redoContextMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Redo();
        }

        private void undoContextMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }

        private void boldContextMenuItem1_Click(object sender, EventArgs e)
        {
            SetBold();
        }

        private void italicContextMenuItem1_Click(object sender, EventArgs e)
        {
            SetItalic();
        }

        private void underlineContextMenuItem1_Click(object sender, EventArgs e)
        {
            SetUnderline();
        }

        private void addBulletsContextMenuItem_Click(object sender, EventArgs e)
        {
            ToggleBulletSelection();
        }

        private void increaseIndentContextMenuItem1_Click(object sender, EventArgs e)
        {
            IncreaseIndent();
        }

        private void decreaseIndentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DecreaseIndent();
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (IsUrlValid(richTextBox.SelectedText))
            {
                browseContextMenuItem.Enabled = true;
                browseContextMenuItem.Visible = true;
                toolStripSeparator21.Visible = true;
            }
            else
            {
                browseContextMenuItem.Enabled = false;
                browseContextMenuItem.Visible = false;
                toolStripSeparator21.Visible = false;
            }
        }

        private void leftJustifyContextMenuItem_Click(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Left);
        }

        private void centreJustifyContextMenuItem_Click(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Center);
        }

        private void rigthJustifyContextMenuItem_Click(object sender, EventArgs e)
        {
            SetAlignment(HorizontalAlignment.Right);
        }

        private void invisibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.BackColor = Color.White;
            richTextBox.ForeColor = Color.White;
        }
        
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_findForm == null)
            {
                _findForm = new FindAndReplaceForm(richTextBox);                
            }

            if (!_findForm.Visible)
            {                
                _findForm.Show();
                _findForm.SetPosition(Location.X, Location.Y);
            }
        }        

        private void helpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(HelpUrl);
            }
            catch
            {
                MessageBox.Show(@"There was an loading the specified link.", @"Error loading Link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cutContextMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Cut();
        }

        private void copyContextMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void pasteContextMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Paste();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void browseContextMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(richTextBox.SelectedText);
            }
            catch
            {
                MessageBox.Show(@"There was an loading the specified link.", @"Error loading Link", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exportFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (exportFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                richTextBox.SaveFile(exportFileDialog.FileName);
            }
            catch (IOException)
            {
                MessageBox.Show(@"There was an error exporting the document to a Rich Text File", @"Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void removeCachedPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_cachedPassword != null)
            {
                MessageBox.Show(@"Cached Passwords Removed", @"Cached Passwords Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            _cachedPassword = null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void addNotebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (var addNoteBook = new AddNotebook())
            //{
            //    if (addNoteBook.ShowDialog() != DialogResult.OK) return;

            //    if (!_settings.NotebookCollection.IsExists(addNoteBook.NoteBookName))
            //    {
            //        _settings.NotebookCollection.CreateNotebook(addNoteBook.NoteBookName);

            //        var menuItem = new ToolStripMenuItem
            //        {
            //            Text = addNoteBook.NoteBookName,
            //            Tag = addNoteBook.NoteBookName,
            //            Name = addNoteBook.NoteBookName
            //        };

            //        notebooksToolStripMenuItem.Click += NotebookMenuItemClickHandler;
            //        notebooksToolStripMenuItem.DropDownItems.Add(menuItem);
            //    }
            //    else
            //    {
            //        MessageBox.Show(@"A notebook with this name already exists.", @"Notebook Already Exists",
            //            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    }
            //}
        }

        private void NotebookMenuItemClickHandler(object sender, EventArgs e)
        {
            //var clickedItem = (ToolStripMenuItem)sender;

            //if (!File.Exists(clickedItem.Text))
            //{
            //    MessageBox.Show(@"Could not load the file : " + clickedItem.Text, @"Could not load file.", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);

            //    _settings.RecentFileList.Remove(clickedItem.Text);
            //    recentFilesToolStripMenuItem.DropDownItems.Remove(clickedItem);

            //    return;
            //}

            //LoadDocument(clickedItem.Text);
        }

        private void clearAllNotebooksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Are you sure you want to delete all the notebooks?", @"Clear notebooks?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                notebooksToolStripMenuItem.DropDownItems.Clear();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private void removeNotebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (var removeNotebook = new RemoveNotebook(_settings.NotebookCollection.RetrieveNotebookNames()))
            //{
            //    if (removeNotebook.ShowDialog() != DialogResult.OK) return;

            //    var selected = removeNotebook.SelectedItems;

            //    notebooksToolStripMenuItem.DropDownItems.Clear();

            //    foreach (var item in selected)
            //    {
            //        _settings.NotebookCollection.RemoveNotebook(item);                   
            //    }

            //    foreach (var newList in _settings.NotebookCollection.RetrieveNotebookNames())
            //    {
            //        var menuItem = new ToolStripMenuItem
            //        {
            //            Text = newList,
            //            Tag = newList,
            //            Name = newList
            //        };

            //        notebooksToolStripMenuItem.Click += NotebookMenuItemClickHandler;
            //        notebooksToolStripMenuItem.DropDownItems.Add(menuItem);
            //    }
            //}
        }

        private void generatePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var passwordGenerator = new PasswordGeneratorForm())
            {
                passwordGenerator.ShowDialog();
                richTextBox.SelectedText = passwordGenerator.Password;
            }
        }

        
    }
}

