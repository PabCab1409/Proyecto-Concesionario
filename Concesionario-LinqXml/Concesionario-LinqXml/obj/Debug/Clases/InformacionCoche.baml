   M S B A M L   `   `   `  ����P  LConcesionario-LinqXml, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:Concesionario_LinqXml.ClasesConcesionario_LinqXml.Clases  5      3    -Concesionario_LinqXml.Clases.InformacionCoche   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:Concesionario_LinqXml.Clases     5        N� Title$  InformacionCoche��5	       � 	Resources 5       �� 
TargetType �� Property �� Value �� Triggers P� Property P� Value   flechaAtras �� ScaleX	 �� ScaleY  	gridCoche��%R  '��       '��     &	  V    &	     �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��$
 25px(�6)   68   �� 5       � ��$ Fonts/#Aileron-Black)�6+   6J    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��$
 Bold"�63   5      5      �� 5   
   " ��6   .�5      ��  � ��
 �   �6;   �� 5       � ��$ Fonts/#Aileron-Black)�6+   6J   �� 5       � ��$
 35px(�6)   68    5      P� 5       �@J�IsMouseOver . . �5      ��  � ��$
 Bold"�63   �� 5       � ��
 �   �63   5      5       �� 5!   
   " ��6    5"      P� 5#       �@J�IsMouseOver . . �5$      ��  � z� 5%      �� 5&      $	 0.9Q�6-   $		 0.9Q�6:   6I   5(      5)      5+      �� 5,   
   " �6    5-      P� 5.       �@J�IsMouseOver . . �5/      ��  � �� �	#FF3F3F3F63   �� 50       � ��
 �����69   �� 51       � z� 52      �� 53      $
 1.05Q�6-   $
	 1.05Q�6;   6K   55      56      58      59      .��5;      � 
�������
 � RowDefinitions
 5=   
   �� 5>      $��70 �6   6*   �� 5?      6   .+�5C   
   � $��0��6   
 5D      �� 5E      $��70 �6    6.   �� 5F      6"    � ColumnDefinitions 5H      �� 5I      $��70 �6#   60   �� 5J      $��* �6#   6/   .+�5M      �� -   5        $��0��5M      $��0��6"   $��Center=�6H   $��10,0,0,0q�6c   .��5N      �� $��/img/flechaAtras.png��6   $��Center=�66   $��40��6Q   $��40��6]   #��[  6h   6�   ��5O      �� 5P       �� 
TargetType" ��6&   .R�5Q      �� $
��None{�6$   �� 6=   6R   5R      5T      5U      � 5X   
   -   5        $��panelImagenCoche��5X      $��1��6    5Y      �� 5Z      $	��550 �6#   61   �� 5[      6%   .+�5_      �� -   5        $��stackPanelInfoCoche��5_   6   $��1��6   $��1��6&   $��30q�6Q   5a      5c       � 5f   
   -   5          �Name$ paginaInformacionCoche��5f      $��2��60   $��2��6D   6W   5g      5h      