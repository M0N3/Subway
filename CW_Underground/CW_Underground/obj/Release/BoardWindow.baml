   M S B A M L   `   `   `  ���� I  ECW_Underground, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:CW_UndergroundCW_Underground       CW_Underground.BoardWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:CW_Underground     -     � Title$  Board��$	��360��$	��340��
������� � 
ResizeMode$ NoResize=�.���  � RowDefinitions �� �� $
��Auto �.+��� -    ��Name$ canvas��$��1��
�����p�$��32��.+��� -   $��AdvertisingTBlock��$�� ��$
��1010��$��32�� �� Padding$
 16,6q�
�����p�
�������$��Impact)��� -    ��Name$ label1��$��The last train left:��$
��Left=�$��
81,187,0,0q�$	��Top=�$��Impact)�$��20(�
�����p��� -   $ lastTrainLeftLB��$
��nope��$
��Left=�$��
10,246,0,0q�$	��Top=�$��Impact)�$��14(�
���p��� -   $ label2��$��Train goes to staion:��$
��Left=�$��
78,107,0,0q�$	��Top=�$��Impact)�$��20(�
�����p��� -   $ 
lastLeftLB��$
��nope��$
��Left=�$��
28,155,0,0q�$	��Top=�$��Impact)�$��16(�
���p��� -   $ lastRightLB��$
��nope��$
��Left=�$��237,155,0,0q�$	��Top=�$��Impact)�$��16(�
���p��� -	   $ label3��$@��:Left Track                                     Right Track��$
��Left=�$��	10,73,0,0q�$	��Top=�$��Impact)�$��20(�
���p��� -
   $ lastTrainRightLB��$"��                        nope��$
��Left=�$��165,246,0,0q�$	��Top=�$��Impact)�$��14(�
���p��� -   $ 
currTimeLB��$��-:-:-��$��120,28,102,0q�$	��Top=�$��Impact)�$��20(�
��� ���
�����p��� -   $ NameLB��$
��Name��$
��Left=�$	��Top=�$��Impact)�$��20(�
��� ���
�����p�$	��113��$��	120,0,0,0q�]� $
��Left=�$��73����.  $	��Top=�$	��334��$��0,-5,0,0q�
�����p� K� ZIndex$	  -1��