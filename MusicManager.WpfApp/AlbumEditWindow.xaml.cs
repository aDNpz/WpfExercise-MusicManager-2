﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicManager.WpfApp
{
    /// <summary>
    /// Interaction logic for AlbumEditWindow.xaml
    /// </summary>
    public partial class AlbumEditWindow : Window
    {
        public AlbumEditWindow()
        {
            InitializeComponent();

            ViewModel.CloseAction = () => Close();
        }
    }
}