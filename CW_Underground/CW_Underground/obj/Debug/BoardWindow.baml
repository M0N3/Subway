   M S B A M L   `   `   `  ����I  ECW_Underground, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CW_UndergroundCW_Underground  5   	        CW_Underground.BoardWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:CW_Underground     5   	   -   5          � Title$  Board��5   	   $	��360��6   $	��340��6$   
������� � 
ResizeMode$ NoResize=�6o   .��5	      �  � RowDefinitions 5
   
   �� 5      �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ canvas��5      $��1��6!   
�����p�$��32��6E   .+�5      �� -   5        $��AdvertisingTBlock��5       $�� ��6   $
��1010��69   $��32��6F    �� Padding$
 16,6q�6R   
�����p�
�������$��Impact)�5   B   5      �� 5   
   -   5         ��Name$ label1��5      $��The last train left:��6    $
��Left=�6?   $��
81,187,0,0q�6Z   $	��Top=�6n   $��Impact)�6�   $��20(�6�   
�����p�6�   �� 5   
   -   5        $ lastTrainLeftLB��5      $
��nope��6)   $
��Left=�68   $��
10,246,0,0q�6S   $	��Top=�6g   $��Impact)�6   $��14(�6�   
���p�6�   �� 5   
   -   5        $ label2��5      $��Train goes to staion:��6    $
��Left=�6@   $��
78,107,0,0q�6[   $	��Top=�6o   $��Impact)�6�   $��20(�6�   
�����p�6�   �� 5   
   -   5        $ 
lastLeftLB��5      $
��nope��6$   $
��Left=�63   $��
28,155,0,0q�6N   $	��Top=�6b   $��Impact)�6z   $��16(�6�   
���p�6�   �� 5   
   -   5        $ lastRightLB��5      $
��nope��6%   $
��Left=�64   $��237,155,0,0q�6O   $	��Top=�6d   $��Impact)�6|   $��16(�6�   
���p�6�   �� 5   
   -	   5        $ label3��5      $@��:Left Track                                     Right Track��6    $
��Left=�6e   $��	10,73,0,0q�6�   $	��Top=�6�   $��Impact)�6�   $��20(�6�   
���p�6�   �� 5   
   -
   5        $ lastTrainRightLB��5      $"��                        nope��6*   $
��Left=�6Q   $��165,246,0,0q�6l   $	��Top=�6�   $��Impact)�6�   $��14(�6�   
���p�6�   �� 5   
   -   5        $ 
currTimeLB��5      $��-:-:-��6$   $��120,28,102,0q�64   $	��Top=�6J   $��Impact)�6b   $��20(�6v   
��� ���
�����p�6�   �� 5   
   -   5        $ NameLB��5      $
��Name��6    $
��Left=�6/   $	��Top=�6J   $��Impact)�6b   $��20(�6v   
��� ���
�����p�$	��113��6�   $��	120,0,0,0q�6�   ]� 5   
   $
��Left=�6   $��73��6/   ��.  $	��Top=�6Q   $	��334��6i   $��0,-5,0,0q�6u   
�����p� K� ZIndex$	  -1��6�   5      5      