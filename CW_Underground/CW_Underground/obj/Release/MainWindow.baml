   M S B A M L   `   `   `  ����   CW_Underground    CW_Underground.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -   $��
mainWindow��  � Title$  Command Centre��$	��700��$
��1000�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�.���  � RowDefinitions �� -    �Name$ rowField��$
��auto � � ColumnDefinitions �� $
��auto ��� -   $ columnField��$
��auto �.+��� -   $��
menuCanvas��$	��300��$
��Left=�$��Center=�$��0��
�����p��� -   $��fieldCanvas��$��Center=�$��Center=�$��1��#��� ���� -   $��CreateSubwayLineBT��$��Create railway��$
��Left=�$��0,415,0,-88q�$	��Top=�$	��150��$��0��$��50��$��Impact)�
�����p�$��20(��� -   $��SubwayLinesLBox��$
��Left=�$	��200��$��0,92,0,0q�$	��Top=�$	��300��$��0��
�������$��Impact)�
���   �� -   $��TooNearStationPP�� � 	StaysOpen .  $��Mouse=�$	��150��$
��Fade=� � AllowsTransparency . .���� 
����ح�$��2q�
�������$	��200��._��� $��10q�$��WrapWithOverflow=�$	��150��Too near for another station!� -	   $��TooNearPathPP�� .  $��Mouse=�$	��150��$
��Fade=� . .���� 
����ح�$��2q�
�������$	��200��._��� $��10q�$��WrapWithOverflow=�$	��150��Too near for another railway!� -
   $��CrossesStationPP�� .  $��Mouse=�$	��170��$
��Fade=� . .���� 
����ح�$��2q�
�������$	��200��._��� $��23,10q�$��WrapWithOverflow=�$	��150��1/The railway does not have to cross the station!�� -   $��AddTrain��$��	Add Train��$
��Left=�$��0,465,0,-131q�$��0��$	��Top=�$	��150��$��50��$��Impact)�
�����p�$��20(��� -    ��Name$ runBT��$��
Start/Stop��$
��Left=�$��0,365,0,-47q�$	��Top=�$	��300��$��50��$��Impact)�
�����p�$��20(��� -   	 ��Name$	 timeTBox��$
��Left=�$��23��$��	0,292,0,0q�
 �� TextWrapping$

 Wrap=�$��00:00:00��$	��Top=�$	��300���� -   $ removeSubwayLineBT��$��Remove Railway��$
��Left=�$��150,415,0,-90q�$	��Top=�$	��150��$��50��$��Impact)�
�����p�$��20(��� -   $ removeTrainBT��$��Remove Train��$
��Left=�$��150,465,0,-130q�$	��Top=�$	��150��$��50��$��Impact)�
�����p�$��20(�$��ExtraCondensed&��� -   $ violationBT��$��Get Violations��$
��Left=�$��	0,315,0,0q�$	��Top=�$	��150��$��50��$��Impact)�
�����p�$��20(��� -    ��Name$ 
timeTBlock��$
��Left=�$
��Wrap=�$��00:00:00��$	��Top=�$��0,-8,0,0q�$��82(�
��� ���$	��300��$��Impact)��� -   $ HelpBT��$
��Help��$
��Left=�$��150,315,0,0q�$	��Top=�$	��150��$��50��$��Impact)�
�����p�$��20(�]� $
��Left=�$	��228����.  $��-6,515,0,-342q�$	��Top=�$	��304������  �� ImageSource$ big_logo.png��