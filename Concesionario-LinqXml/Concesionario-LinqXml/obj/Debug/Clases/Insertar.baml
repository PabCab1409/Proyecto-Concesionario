   M S B A M L   `   `   `  ����P  LConcesionario-LinqXml, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Concesionario_LinqXml.ClasesConcesionario_LinqXml.Clases  5      +    %Concesionario_LinqXml.Clases.Insertar   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:Concesionario_LinqXml.Clases     5        N� Title$  Insertar��5	       � 	Resources 5
       �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value   flechaAtras �� ScaleX	 �� ScaleY  ComboBoxToggleButton
 �� 
TargetType � ColumnDefinitions ��Name �� CornerRadius F�Name �� Triggers �� 
TargetName  ComboBoxTextBox �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth  �� MaxDropDownHeight! K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation"   DirectionalNavigation# P� 
SourceName��%  '��       '�� �     '�� �    &	      &	 �    &	 �    '�� :    7  7  �� 5   
   " ��6   .�5      ��  � ��$
 25px(�6)   68   �� 5       � ��$ 
75,10,0,10q�6'   6<   �� 5       � ��$ Center=�69   6J   5      �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��$
 25px(�6)   68   �� 5       � ��$ Fonts/#Aileron-Black)�6+   6J    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��$
 Bold"�63   5      5      �� 5   
   " ��6   .�5      ��  � ��
 �   �6;   �� 5       � ��$ Fonts/#Aileron-Black)�6+   6J   �� 5       � ��$
 35px(�6)   68    5      P� 5       �@J�IsMouseOver . . �5       ��  � ��$
 Bold"�63   �� 5!       � ��
 �   �63   5"      5$      �� 5%   
   " ��6    5&      P� 5'       �@J�IsMouseOver . . �5(      ��  � z� 5)      �� 5*      $	 0.9Q�6-   $		 0.9Q�6:   6I   5,      5-      5/      �� 51   
   "
 d�67   .R�52      �  53      �� 54      �� 55      $��20 �6'   .+�57      �� $ Border��58      $��2��59      $ 0��5:      ���	#FF3F3F3F���	#FF97A0A5$��1q�5=      �� 5>      $��0��5?      $ 0��5@      $��1q�5A      ���	#FF3F3F3F���	#FF97A0A5$��0,0,1,0q�5D      F� 5E      $ Arrow��5F      $��1��5G      
�������$��Center=�5I      $��Center=�5J      =����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	5K      5M       5N      P� 5O       �@J�IsMouseOver . . �5P      �� $ Border��6    � �� �#8080806G   5Q      P� 5R       �@d�	IsChecked$
 trueY�6<   . �5S      �� $ Border��6    � �� �#E0E0E06G   5T      P� 5U       � {� .  . �5V      �� $ Border��6    � �� �#EEEEEE6G   �� 5W      $ Border��6    � �� �#AAAAAA6H   �� 5X       � �� �#88888863   �� 5Y      $ Arrow��6    � �� �#8888886@   5Z      5\      �� 5]   
   "
 ��62   .R�5^      �� $ PART_ContentHost��6   |�.  #��z��6A   5_      �� 5`   
   " ��6*   .�5a      ��  �@J�SnapsToDevicePixels . 64   �� 5b       �@�OverridesDefaultStyle . 66   �� 5c       � ��$
 Auto=�6K   �� 5d       � ��$
 Auto=�6I   �� 5e       � �� . 6>   �� 5f       � ��$	 120��6)   �� 5g       � ��$ 20��6*   �� 5h       � ��
 �����6+   �� 5i       � �� 5j      �� 5k      "
 ��6&   .R�5l      � .+�5m      d� $��ToggleButton��5n   !   8��  $��2��5p   !   |�.  $ Press=�5s   !    5r   !   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5t      �� 5u      $��ContentSite��6/    .  #��z 6\   #��z 5v   !   #��z��5w   !   $��3,3,23,3q�5x   !   $��Center=�5y   !   $
��Left=�5z   !   �� 5{      $ PART_EditableTextBox��6&   8�� $
��Left=�5~   !   $��Center=�5   !   $��3,3,23,3q�5�   !   |�. ���	#FF3F3F3F
��� � �$y�Hidden=�5�   !   # z 5�   !   ��5|   !   Z� 5�   !   � 5�      $��Popup��5�   !   $��Bottom=�5�   !   #��z 5�   !    . |�.  $��Slide=�5�   !   .��5�   "   � $��DropDown��6'    . #��z 5�   !   #��z  5�   !   .+�5�   &   �� $ DropDownBorder��5�   !   ���	#FF3F3F3F$��1q�5�   !   ���#8888885�   !   �� 5�   &   $��4,6,4,6q�63    . .��5�   *   �� ! . $" 	Contained=�6H   5�   '   5�   #   5�      5�       5�      P� 5�       �@��HasItems .  . �5�   "   �� $ DropDownBorder��6)    � ��$ 95��6Z   5�      P� 5�       � {� .  . �5�   "   ��  � �� �#8888886?   5�      P� 5�       �@��
IsGrouping . . �5�   "   ��  � �� .  6R   5�      P� 5�      $# Popup��6&    �@�AllowsTransparency . . �5�   "   �� $ DropDownBorder��6)    �@��CornerRadius$ 0��6]   �� 5�   "   $ DropDownBorder��6)    � ��$ 0,2,0,0q�6W   5�      P� 5�       �@��
IsEditable . . �5�   "   ��  � �� .  6>   �� 5�   "   $ PART_EditableTextBox��6)    � y�$ Visible=�6a   �� 5�   "   $ ContentSite��6)    � y�$ Hidden=�6X   5�      5�      5�       5�      5�      5�      .��5�      � ���#f5f5f5$ � RowDefinitions$ 5�   
   �� 5�      $��70 �6   6*   �� 5�      6    5�   
   �� 5�      $	��450 �6   6-   �� 5�      6!   .+�5�   
   � $��0��6   $ 5�      �� 5�      $��70 �6    6.   �� 5�      6"    5�      �� 5�      $��70 �6#   60   �� 5�      $��* �6#   6/   .+�5�      �� -   5        $��0��5�      $��0��6"   $��Center=�6N   $��10,0,0,0q�6i   .��5�      �� $��/img/flechaAtras.png��6   $��Center=�66   $��40��6Q   $��40��6]   #��[  6h   6�   ��5�      �� 5�      "
 ��6&   .R�5�      �� $
��None{�6$   �� 6=   6R   5�      5�      5�      � 5�   
   -   5        $��gridCocheInsertado��5�      $��1��6   $��30q�66   5�      � 5�   
   $��1��6   $��2��6   $ 5�      �� 5�      $��50 �6    6.   �� 5�      6#    5�      �� 5�      $	��350 �6#   61   �� 5�      6%   .+�5�      �� $-��'Introduce los datos para el nuevo coche��6   $��2��6F   6\   �� 5�      $��20q�6   $��1��6%   .+�5�      �� .��6   MARCA6   �� 5�      -   5        $��comboBoxMarca��5�   -   $��0,0,0,10q�6   6v   �� 5�      .��6   MODELO6    �� 5�      -   5        $��comboBoxModelo��5�   -   $��0,0,0,10q�6   6x   �� 5�      .��6   	VERSION6!   �� 5�      -   5        $��comboBoxVersion��5�   -   $��0,0,0,10q�6   6z   �� 5�      % �� Orientation$% 
Horizontal=�6   .+�5�      �� .��6   PRECIO6$   �� 5�      -   5        $��tbPrecio��5�      $��Center=�6/   $��30��6Q   $	��130��6]   6k   5�      �� 5�      .��6   COMBUSTIBLE6%   �� 5�      -   5        $��comboBoxCombustible��5�      .@�5�      �� $��Diesel��6#   66   �� 5�      $��Gasolina��6#   68   5�      5�      �� 5�      $��1��6   $��1��6&   $��50q�66   .+�5�      �� $��1��6   $��1��6%   $��Center=�65   .��6R   Extras6Z   �� 5�      
���   �	& �� X1$	& 400��6&   $��Center=�6/   6N   �� 5�      -   5        ' ��Name$' checkBoxEx1��5�      .��60   Llantas 19 pulgadas6E   �� 5�      -	   5        $' checkBoxEx2��5�      .��60   Asientos de cuero6C   �� 5�      -
   5        $' checkBoxEx3��5�      .��60   	Faros led6;   �� 5�      -   5        $' checkBoxEx4��5�      .��60   Porton68   5      �� 5     -   5        $��1��5     $��1��6%   $��25q�62   $��INSERTAR��6>   ( � Cursor$
( Hand��6Q   $��Bottom=�6}   $��Right=�6�   ��5     �� 5     "
 ��6&   .R�5     �� $
��None{�6$   
���   �$
��58px(�6N   $��Center=�6^   �� 6}   6�   5     5     5	      � 5
  
   -   5        )  �Name$) paginaInsertar��5
     $��2��6(   $��2��6<   6O   5     5     