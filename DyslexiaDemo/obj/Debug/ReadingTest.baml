   M S B A M L   `   `   `  ����G  CDyslexiaDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:DyslexiaDemoDyslexiaDemo  5          DyslexiaDemo.ReadingTest   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:DyslexiaDemo     5      J FWpfAnimatedGif, Version=1.4.14.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5      $	��720��5
      $	��720��5      $
��1250��5      $
��1250��5      $
��1250��5        N� 
Background
  ����� N� Title$ ReadingTest��5      .��5      � ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ 	#FFB8E3F3��6    � Offset$ 0Q�61   � 5      $ 	#FFDCF0F5��6   $ 1Q�61   5      .+�5   
   � -   5        $��BackCanvasGrid��5      $
��Left=�6%   $	��640��6@   $��	10,10,0,0q�6M   $	��Top=�6`   $
��1230��6x   $	��640��6�   $	��640��6�   $
��1230��6�   $
��1230��6�   ���	#FFFDFDFD.+�5      �� -   5         ��Name$ BackenglishImage��5      $
��Left=�6.   $��10,0,0,0q�6I   $
��1210��6[   $	��640��6h   $	��Top=�6u   $��englishText.png��6�   $y�Visible=�6�   �� 5      -   5        $ BackarabicImage��5      $
��Left=�6-   $��10,0,0,0q�6H   $
��1210��6Z   $	��640��6g   $	��Top=�6t   $��arabicText.png��6�   $y�Hidden=�6�   �� 5      -   5         ��Name$ 
BackCanvas��5      $
��Left=�6)   $	��640��6D   $��0,0,0,0q�6Q   $	��Top=�6b   $
��1230��6z   $	��640��6�   $	��640��6�   $
��1230��6�   $
��1230��6�   5      � 5   
   -   5        $��
CanvasGrid��5      $y�Visible=�6!   $
��Left=�66   $	��640��6Q   $��	10,10,0,0q�6^   $	��Top=�6q   $
��1230��6�   $	��640��6�   $	��640��6�   $
��1230��6�   $
��1230��6�   ���	#FFFDFDFD.+�5      �� -   5        $ englishImage��5      $
��Left=�6*   $��10,0,0,0q�6E   $
��1210��6W   $	��640��6d   $	��Top=�6q   $��englishText.png��6�   $y�Visible=�6�   �� 5       -   5        $ arabicImage��5       $
��Left=�6)   $��10,0,0,0q�6D   $
��1210��6V   $	��640��6c   $	��Top=�6p   $��arabicText.png��6�   $y�Hidden=�6�   �� 5!      -   5        $ FrontCanvas��5!      $
��Left=�6*   $	��640��6E   $��0,0,0,0q�6R   $	��Top=�6c   $
��1230��6{   $	��640��6�   $	��640��6�   $
��1230��6�   $
��1230��6�   �� 5#      -	   5         ��Name$ 	textBlock��5#      $
��Left=�6+   $��10,0,0,0q�6F   $
��Wrap=�6X   $����Go play the bear and sheep will come for me to go and see the cat for dog my tree has bear and we want candy for look at the grass to up down for to look down up we have see you can show cat dog up to play come go and we                                     will to see any come play and up go the cat has not look for you and to it will be from see and play down with to the go has fly and go the bear will be on the have go to we will play look and has to go be the sheep cat dog is far go come is to we go when has bear and go was up down we call for one me you cat dog play we has up down me the up is to go be for me you are is to cat��6l   $	��Top=�5%      $��40(�6,   $
��1210��6:   $	��640��6G   $��Global Monospace)�6T   ���	#FF910FEB	 �� TextAlignment$	 Justify=�6�   $y�Hidden=�6�   5&      �� 5)   
   -
   5        
 ��Name$
 startbutton��5)      $��Start Recording��6&   $��Center=�6[   $��	0,670,0,0q�6x   $	��Top=�6�   $	��400��6�   $��35��6�   $��16(�6�   $
��Bold"�6�   ��5*      
� 5+      .��5,      � $ 	#FF7CACC4��6#   $ 1Q�65   � 5-      $ 	#FFA7CEE0��6#   5.      50      �� 52   
   -   5        $
 submitbutton��52      $
��DONE��6'   $��Center=�6R   $��	0,670,0,0q�6o   $	��Top=�6�   $	��400��6�   $��35��6�   $��16(�6�   $
��Bold"�6�   $��Center=�6�   $y�Hidden=�6�   ��53      
� 54      .��55      � $ 	#FF7CACC4��6#   $ 1Q�65   � 56      $ 	#FFA7CEE0��6#   57      59      �� 5:   
   -   5        $ loadingImage��5:      $	��150��6&   "  WpfAnimatedGif.ImageBehavior   AnimatedSource$ loading.gif��63   $y�Hidden=�6b   $��Center=�6v   $��loading.gif��6�   � 5;   
   $	��217��6   $��60��6   $��Bottom=�6'   $
��Left=�6B   $��15,0,0,8q�6]   .+�5<      	� -   5        $��English��5<      $
��Left=�5>      $��	10,15,0,0q�5?      $	��Top=�5@      $��21.333(�6    $��Center=�62   $��8,0,0,0q�6T    d� 	IsChecked$
 TrueY�6|   ��6f   Z� 6|   	� 5A      -   5        $��Arabic��5A      $
��Left=�5C      $��128,15,0,18q�5D      $��21.333(�6   $��Center=�6/   $��8,0,0,0q�6Q   ��6c   Z� 5E      5F      5G      