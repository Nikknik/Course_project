   M S B A M L   `   `   `  ����A  =Crisis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CrisisCrisis  5   	       Crisis.Window2   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Crisis     5   	   $	��400��5   
   $	��400��6   $	��250��5	   	   $	��250��6   .��5
      �   � DataContext  5   
   � 5      -   5        	data4�� � Source$ 	\item.txt?�5   -    � RowDefinitions 5   
   �� 5      $��1* �6   6*   �� 5      $
��0.2* �6   6,   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5      $��7q�6%      AutoGenerateColumns .  ��60   ��  �� XPath$ answer��   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      -   5        column1��,  &System.Windows.Controls.DataGridColumn   Header$	 №��5   6      Width5  /System.Windows.Controls.DataGridLengthConverter$ 1* 6c   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6A   �� $ column1��6c     5      -   5        column2��$ 5* 5   X   	 66   �� $ column2��6X     5      -   5        column3��$ 5* 5   X   	 66   �� $ column3��6X   5      �� 5   
   -   5        
 ��Name$	
 add��5      $#��Добавить строку��6   $��Right=�68   $��0,0,30,0q�6T   $��1��6g   $��Center=�6t   $	��100��6�   6�   �� 5   
   -   5        $

 save��5      $��Сохранить��6   $��Right=�63   $��	0,0,150,0q�6O   $��1��6c   $��Center=�6p   $	��100��6�   6�   5      5      