// --------------------------------------------------------------------------------------------------------
// 
// GIS-Editor - a tool to create, visualize, edit and archive samples within a geographical environment.
// Copyright (C) 2011 by Wolfgang Reichert, Botanische Staatssammlung München, mailto: reichert@bsm.mwn.de
//
// This program is free software; you can redistribute it and/or modify it under the terms of the 
// GNU General Public License as published by the Free Software Foundation; 
// either version 2 of the License, or (at your option) any later version. 
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details. 
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, write to the Free Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110, USA
//
// --------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Diagnostics;

namespace WpfSamplingPlotPage
{

    /// <summary>
    /// Interaktionslogik für Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        #region Constants

        /// <summary>
        /// License text with terms and conditions.
        /// </summary>
        internal const string StrLicense =
        "                                                                               " + WpfControl.StrCR +
        "                     GNU GENERAL PUBLIC LICENSE                                " + WpfControl.StrCR +
        "                        Version 2, June 1991                                   " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "  Copyright (C) 1989, 1991 Free Software Foundation, Inc.,                     " + WpfControl.StrCR +
        "  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA                   " + WpfControl.StrCR +
        "  Everyone is permitted to copy and distribute verbatim copies                 " + WpfControl.StrCR +
        "  of this license document, but changing it is not allowed.                    " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "                             Preamble                                          " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   The licenses for most software are designed to take away your               " + WpfControl.StrCR +
        " freedom to share and change it.  By contrast, the GNU General Public          " + WpfControl.StrCR +
        " License is intended to guarantee your freedom to share and change free        " + WpfControl.StrCR +
        " software--to make sure the software is free for all its users.  This          " + WpfControl.StrCR +
        " General Public License applies to most of the Free Software                   " + WpfControl.StrCR +
        " Foundation's software and to any other program whose authors commit to        " + WpfControl.StrCR +
        " using it.  (Some other Free Software Foundation software is covered by        " + WpfControl.StrCR +
        " the GNU Lesser General Public License instead.)  You can apply it to          " + WpfControl.StrCR +
        " your programs, too.                                                           " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   When we speak of free software, we are referring to freedom, not            " + WpfControl.StrCR +
        " price.  Our General Public Licenses are designed to make sure that you        " + WpfControl.StrCR +
        " have the freedom to distribute copies of free software (and charge for        " + WpfControl.StrCR +
        " this service if you wish), that you receive source code or can get it         " + WpfControl.StrCR +
        " if you want it, that you can change the software or use pieces of it          " + WpfControl.StrCR +
        " in new free programs; and that you know you can do these things.              " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   To protect your rights, we need to make restrictions that forbid            " + WpfControl.StrCR +
        " anyone to deny you these rights or to ask you to surrender the rights.        " + WpfControl.StrCR +
        " These restrictions translate to certain responsibilities for you if you       " + WpfControl.StrCR +
        " distribute copies of the software, or if you modify it.                       " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   For example, if you distribute copies of such a program, whether            " + WpfControl.StrCR +
        " gratis or for a fee, you must give the recipients all the rights that         " + WpfControl.StrCR +
        " you have.  You must make sure that they, too, receive or can get the          " + WpfControl.StrCR +
        " source code.  And you must show them these terms so they know their           " + WpfControl.StrCR +
        " rights.                                                                       " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   We protect your rights with two steps: (1) copyright the software, and      " + WpfControl.StrCR +
        " (2) offer you this license which gives you legal permission to copy,          " + WpfControl.StrCR +
        " distribute and/or modify the software.                                        " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   Also, for each author's protection and ours, we want to make certain        " + WpfControl.StrCR +
        " that everyone understands that there is no warranty for this free             " + WpfControl.StrCR +
        " software.  If the software is modified by someone else and passed on, we      " + WpfControl.StrCR +
        " want its recipients to know that what they have is not the original, so       " + WpfControl.StrCR +
        " that any problems introduced by others will not reflect on the original       " + WpfControl.StrCR +
        " authors' reputations.                                                         " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   Finally, any free program is threatened constantly by software              " + WpfControl.StrCR +
        " patents.  We wish to avoid the danger that redistributors of a free           " + WpfControl.StrCR +
        " program will individually obtain patent licenses, in effect making the        " + WpfControl.StrCR +
        " program proprietary.  To prevent this, we have made it clear that any         " + WpfControl.StrCR +
        " patent must be licensed for everyone's free use or not licensed at all.       " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   The precise terms and conditions for copying, distribution and              " + WpfControl.StrCR +
        " modification follow.                                                          " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "                     GNU GENERAL PUBLIC LICENSE                                " + WpfControl.StrCR +
        "    TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION            " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   0. This License applies to any program or other work which contains         " + WpfControl.StrCR +
        " a notice placed by the copyright holder saying it may be distributed          " + WpfControl.StrCR +
        " under the terms of this General Public License.  The \"Program\", below,      " + WpfControl.StrCR +
        " refers to any such program or work, and a \"work based on the Program\"       " + WpfControl.StrCR +
        " means either the Program or any derivative work under copyright law:          " + WpfControl.StrCR +
        " that is to say, a work containing the Program or a portion of it,             " + WpfControl.StrCR +
        " either verbatim or with modifications and/or translated into another          " + WpfControl.StrCR +
        " language.  (Hereinafter, translation is included without limitation in        " + WpfControl.StrCR +
        " the term \"modification\".)  Each licensee is addressed as \"you\".           " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " Activities other than copying, distribution and modification are not          " + WpfControl.StrCR +
        " covered by this License; they are outside its scope.  The act of              " + WpfControl.StrCR +
        " running the Program is not restricted, and the output from the Program        " + WpfControl.StrCR +
        " is covered only if its contents constitute a work based on the                " + WpfControl.StrCR +
        " Program (independent of having been made by running the Program).             " + WpfControl.StrCR +
        " Whether that is true depends on what the Program does.                        " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   1. You may copy and distribute verbatim copies of the Program's             " + WpfControl.StrCR +
        " source code as you receive it, in any medium, provided that you               " + WpfControl.StrCR +
        " conspicuously and appropriately publish on each copy an appropriate           " + WpfControl.StrCR +
        " copyright notice and disclaimer of warranty; keep intact all the              " + WpfControl.StrCR +
        " notices that refer to this License and to the absence of any warranty;        " + WpfControl.StrCR +
        " and give any other recipients of the Program a copy of this License           " + WpfControl.StrCR +
        " along with the Program.                                                       " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " You may charge a fee for the physical act of transferring a copy, and         " + WpfControl.StrCR +
        " you may at your option offer warranty protection in exchange for a fee.       " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   2. You may modify your copy or copies of the Program or any portion         " + WpfControl.StrCR +
        " of it, thus forming a work based on the Program, and copy and                 " + WpfControl.StrCR +
        " distribute such modifications or work under the terms of Section 1            " + WpfControl.StrCR +
        " above, provided that you also meet all of these conditions:                   " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "     a) You must cause the modified files to carry prominent notices           " + WpfControl.StrCR +
        "     stating that you changed the files and the date of any change.            " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "     b) You must cause any work that you distribute or publish, that in        " + WpfControl.StrCR +
        "     whole or in part contains or is derived from the Program or any           " + WpfControl.StrCR +
        "     part thereof, to be licensed as a whole at no charge to all third         " + WpfControl.StrCR +
        "     parties under the terms of this License.                                  " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "     c) If the modified program normally reads commands interactively          " + WpfControl.StrCR +
        "     when run, you must cause it, when started running for such                " + WpfControl.StrCR +
        "     interactive use in the most ordinary way, to print or display an          " + WpfControl.StrCR +
        "     announcement including an appropriate copyright notice and a              " + WpfControl.StrCR +
        "     notice that there is no warranty (or else, saying that you provide        " + WpfControl.StrCR +
        "     a warranty) and that users may redistribute the program under             " + WpfControl.StrCR +
        "     these conditions, and telling the user how to view a copy of this         " + WpfControl.StrCR +
        "     License.  (Exception: if the Program itself is interactive but            " + WpfControl.StrCR +
        "     does not normally print such an announcement, your work based on          " + WpfControl.StrCR +
        "     the Program is not required to print an announcement.)                    " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " These requirements apply to the modified work as a whole.  If                 " + WpfControl.StrCR +
        " identifiable sections of that work are not derived from the Program,          " + WpfControl.StrCR +
        " and can be reasonably considered independent and separate works in            " + WpfControl.StrCR +
        " themselves, then this License, and its terms, do not apply to those           " + WpfControl.StrCR +
        " sections when you distribute them as separate works.  But when you            " + WpfControl.StrCR +
        " distribute the same sections as part of a whole which is a work based         " + WpfControl.StrCR +
        " on the Program, the distribution of the whole must be on the terms of         " + WpfControl.StrCR +
        " this License, whose permissions for other licensees extend to the             " + WpfControl.StrCR +
        " entire whole, and thus to each and every part regardless of who wrote it.     " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " Thus, it is not the intent of this section to claim rights or contest         " + WpfControl.StrCR +
        " your rights to work written entirely by you; rather, the intent is to         " + WpfControl.StrCR +
        " exercise the right to control the distribution of derivative or               " + WpfControl.StrCR +
        " collective works based on the Program.                                        " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " In addition, mere aggregation of another work not based on the Program        " + WpfControl.StrCR +
        " with the Program (or with a work based on the Program) on a volume of         " + WpfControl.StrCR +
        " a storage or distribution medium does not bring the other work under          " + WpfControl.StrCR +
        " the scope of this License.                                                    " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   3. You may copy and distribute the Program (or a work based on it,          " + WpfControl.StrCR +
        " under Section 2) in object code or executable form under the terms of         " + WpfControl.StrCR +
        " Sections 1 and 2 above provided that you also do one of the following:        " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "     a) Accompany it with the complete corresponding machine-readable          " + WpfControl.StrCR +
        "     source code, which must be distributed under the terms of Sections 1      " + WpfControl.StrCR +
        "     and 2 above on a medium customarily used for software interchange; or,    " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "     b) Accompany it with a written offer, valid for at least three            " + WpfControl.StrCR +
        "     years, to give any third party, for a charge no more than your            " + WpfControl.StrCR +
        "     cost of physically performing source distribution, a complete             " + WpfControl.StrCR +
        "     machine-readable copy of the corresponding source code, to be             " + WpfControl.StrCR +
        "     distributed under the terms of Sections 1 and 2 above on a medium         " + WpfControl.StrCR +
        "     customarily used for software interchange; or,                            " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "     c) Accompany it with the information you received as to the offer         " + WpfControl.StrCR +
        "     to distribute corresponding source code.  (This alternative is            " + WpfControl.StrCR +
        "     allowed only for noncommercial distribution and only if you               " + WpfControl.StrCR +
        "     received the program in object code or executable form with such          " + WpfControl.StrCR +
        "     an offer, in accord with Subsection b above.)                             " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " The source code for a work means the preferred form of the work for           " + WpfControl.StrCR +
        " making modifications to it.  For an executable work, complete source          " + WpfControl.StrCR +
        " code means all the source code for all modules it contains, plus any          " + WpfControl.StrCR +
        " associated interface definition files, plus the scripts used to               " + WpfControl.StrCR +
        " control compilation and installation of the executable.  However, as a        " + WpfControl.StrCR +
        " special exception, the source code distributed need not include               " + WpfControl.StrCR +
        " anything that is normally distributed (in either source or binary             " + WpfControl.StrCR +
        " form) with the major components (compiler, kernel, and so on) of the          " + WpfControl.StrCR +
        " operating system on which the executable runs, unless that component          " + WpfControl.StrCR +
        " itself accompanies the executable.                                            " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " If distribution of executable or object code is made by offering              " + WpfControl.StrCR +
        " access to copy from a designated place, then offering equivalent              " + WpfControl.StrCR +
        " access to copy the source code from the same place counts as                  " + WpfControl.StrCR +
        " distribution of the source code, even though third parties are not            " + WpfControl.StrCR +
        " compelled to copy the source along with the object code.                      " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   4. You may not copy, modify, sublicense, or distribute the Program          " + WpfControl.StrCR +
        " except as expressly provided under this License.  Any attempt                 " + WpfControl.StrCR +
        " otherwise to copy, modify, sublicense or distribute the Program is            " + WpfControl.StrCR +
        " void, and will automatically terminate your rights under this License.        " + WpfControl.StrCR +
        " However, parties who have received copies, or rights, from you under          " + WpfControl.StrCR +
        " this License will not have their licenses terminated so long as such          " + WpfControl.StrCR +
        " parties remain in full compliance.                                            " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   5. You are not required to accept this License, since you have not          " + WpfControl.StrCR +
        " signed it.  However, nothing else grants you permission to modify or          " + WpfControl.StrCR +
        " distribute the Program or its derivative works.  These actions are            " + WpfControl.StrCR +
        " prohibited by law if you do not accept this License.  Therefore, by           " + WpfControl.StrCR +
        " modifying or distributing the Program (or any work based on the               " + WpfControl.StrCR +
        " Program), you indicate your acceptance of this License to do so, and          " + WpfControl.StrCR +
        " all its terms and conditions for copying, distributing or modifying           " + WpfControl.StrCR +
        " the Program or works based on it.                                             " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   6. Each time you redistribute the Program (or any work based on the         " + WpfControl.StrCR +
        " Program), the recipient automatically receives a license from the             " + WpfControl.StrCR +
        " original licensor to copy, distribute or modify the Program subject to        " + WpfControl.StrCR +
        " these terms and conditions.  You may not impose any further                   " + WpfControl.StrCR +
        " restrictions on the recipients' exercise of the rights granted herein.        " + WpfControl.StrCR +
        " You are not responsible for enforcing compliance by third parties to          " + WpfControl.StrCR +
        " this License.                                                                 " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   7. If, as a consequence of a court judgment or allegation of patent         " + WpfControl.StrCR +
        " infringement or for any other reason (not limited to patent issues),          " + WpfControl.StrCR +
        " conditions are imposed on you (whether by court order, agreement or           " + WpfControl.StrCR +
        " otherwise) that contradict the conditions of this License, they do not        " + WpfControl.StrCR +
        " excuse you from the conditions of this License.  If you cannot                " + WpfControl.StrCR +
        " distribute so as to satisfy simultaneously your obligations under this        " + WpfControl.StrCR +
        " License and any other pertinent obligations, then as a consequence you        " + WpfControl.StrCR +
        " may not distribute the Program at all.  For example, if a patent              " + WpfControl.StrCR +
        " license would not permit royalty-free redistribution of the Program by        " + WpfControl.StrCR +
        " all those who receive copies directly or indirectly through you, then         " + WpfControl.StrCR +
        " the only way you could satisfy both it and this License would be to           " + WpfControl.StrCR +
        " refrain entirely from distribution of the Program.                            " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " If any portion of this section is held invalid or unenforceable under         " + WpfControl.StrCR +
        " any particular circumstance, the balance of the section is intended to        " + WpfControl.StrCR +
        " apply and the section as a whole is intended to apply in other                " + WpfControl.StrCR +
        " circumstances.                                                                " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " It is not the purpose of this section to induce you to infringe any           " + WpfControl.StrCR +
        " patents or other property right claims or to contest validity of any          " + WpfControl.StrCR +
        " such claims; this section has the sole purpose of protecting the              " + WpfControl.StrCR +
        " integrity of the free software distribution system, which is                  " + WpfControl.StrCR +
        " implemented by public license practices.  Many people have made               " + WpfControl.StrCR +
        " generous contributions to the wide range of software distributed              " + WpfControl.StrCR +
        " through that system in reliance on consistent application of that             " + WpfControl.StrCR +
        " system; it is up to the author/donor to decide if he or she is willing        " + WpfControl.StrCR +
        " to distribute software through any other system and a licensee cannot         " + WpfControl.StrCR +
        " impose that choice.                                                           " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " This section is intended to make thoroughly clear what is believed to         " + WpfControl.StrCR +
        " be a consequence of the rest of this License.                                 " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   8. If the distribution and/or use of the Program is restricted in           " + WpfControl.StrCR +
        " certain countries either by patents or by copyrighted interfaces, the         " + WpfControl.StrCR +
        " original copyright holder who places the Program under this License           " + WpfControl.StrCR +
        " may add an explicit geographical distribution limitation excluding            " + WpfControl.StrCR +
        " those countries, so that distribution is permitted only in or among           " + WpfControl.StrCR +
        " countries not thus excluded.  In such case, this License incorporates         " + WpfControl.StrCR +
        " the limitation as if written in the body of this License.                     " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   9. The Free Software Foundation may publish revised and/or new versions     " + WpfControl.StrCR +
        " of the General Public License from time to time.  Such new versions will      " + WpfControl.StrCR +
        " be similar in spirit to the present version, but may differ in detail to      " + WpfControl.StrCR +
        " address new problems or concerns.                                             " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        " Each version is given a distinguishing version number.  If the Program        " + WpfControl.StrCR +
        " specifies a version number of this License which applies to it and \"any      " + WpfControl.StrCR +
        " later version\", you have the option of following the terms and conditions    " + WpfControl.StrCR +
        " either of that version or of any later version published by the Free          " + WpfControl.StrCR +
        " Software Foundation.  If the Program does not specify a version number of     " + WpfControl.StrCR +
        " this License, you may choose any version ever published by the Free Software  " + WpfControl.StrCR +
        " Foundation.                                                                   " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   10. If you wish to incorporate parts of the Program into other free         " + WpfControl.StrCR +
        " programs whose distribution conditions are different, write to the author     " + WpfControl.StrCR +
        " to ask for permission.  For software which is copyrighted by the Free         " + WpfControl.StrCR +
        " Software Foundation, write to the Free Software Foundation; we sometimes      " + WpfControl.StrCR +
        " make exceptions for this.  Our decision will be guided by the two goals       " + WpfControl.StrCR +
        " of preserving the free status of all derivatives of our free software and     " + WpfControl.StrCR +
        " of promoting the sharing and reuse of software generally.                     " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "                             NO WARRANTY                                       " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   11. BECAUSE THE PROGRAM IS LICENSED FREE OF CHARGE, THERE IS NO WARRANTY    " + WpfControl.StrCR +
        " FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE LAW.  EXCEPT WHEN      " + WpfControl.StrCR +
        " OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR OTHER PARTIES        " + WpfControl.StrCR +
        " PROVIDE THE PROGRAM \"AS IS\" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED  " + WpfControl.StrCR +
        " OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF          " + WpfControl.StrCR +
        " MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.  THE ENTIRE RISK AS     " + WpfControl.StrCR +
        " TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU.  SHOULD THE        " + WpfControl.StrCR +
        " PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING,      " + WpfControl.StrCR +
        " REPAIR OR CORRECTION.                                                         " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "   12. IN NO EVENT UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING   " + WpfControl.StrCR +
        " WILL ANY COPYRIGHT HOLDER, OR ANY OTHER PARTY WHO MAY MODIFY AND/OR           " + WpfControl.StrCR +
        " REDISTRIBUTE THE PROGRAM AS PERMITTED ABOVE, BE LIABLE TO YOU FOR DAMAGES,    " + WpfControl.StrCR +
        " INCLUDING ANY GENERAL, SPECIAL, INCIDENTAL OR CONSEQUENTIAL DAMAGES ARISING   " + WpfControl.StrCR +
        " OUT OF THE USE OR INABILITY TO USE THE PROGRAM (INCLUDING BUT NOT LIMITED     " + WpfControl.StrCR +
        " TO LOSS OF DATA OR DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY      " + WpfControl.StrCR +
        " YOU OR THIRD PARTIES OR A FAILURE OF THE PROGRAM TO OPERATE WITH ANY OTHER    " + WpfControl.StrCR +
        " PROGRAMS), EVEN IF SUCH HOLDER OR OTHER PARTY HAS BEEN ADVISED OF THE         " + WpfControl.StrCR +
        " POSSIBILITY OF SUCH DAMAGES.                                                  " + WpfControl.StrCR +
        "                                                                               " + WpfControl.StrCR +
        "                      END OF TERMS AND CONDITIONS                              " + WpfControl.StrCR;

        #endregion // Constants
        
        #region Fields

        #endregion // Fields

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class.
        /// </summary>
        public Info()
        {
            InitializeComponent();

            labelInfoLogo.Content = GetImageFromFile("DivWor_EndnurLogo.png");
            labelInfoTitle.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelTitle;     
            labelInfoDescription.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelDescription;
            labelInfoVersion.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelVersion;
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            labelInfoVersionData.Content = version.Substring(0, version.Length - 2);
            labelInfoInstitution.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelInstitution;
            labelInfoInstitutionData.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelInstitutionData;
            labelInfoAuthors.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelAuthors;
            labelInfoAuthorsData.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelAuthorsData;
            labelInfoCopyright.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelCopyright;
            labelInfoCopyrightData.Content = string.Format(WpfSamplingPlotPage.Properties.Resources.InfoLabelCopyrightData, DateTime.Now.Year.ToString());
            labelInfoLicense.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelLicense;
            labelInfoLicenseData.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelLicenseData;
            labelInfoDisclaimer.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelDisclaimer;
            labelInfoDisclaimerData.Content = WpfSamplingPlotPage.Properties.Resources.InfoLabelDisclaimerData;

            // labelInfoDescription.Content = string.Format(WpfSamplingPlotPage.Properties.Resources.InfoLabelDescription.Replace("\\r\\n", "\r\n"),
            //    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            // scrollViewerInfoLicense.Content = StrLicense;
        }

        #endregion // Construction

        #region Event handlers

        /// <summary>
        /// Handles the RequestNavigate event of the Hyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // Navigate to the website submitted in the arguments
            Process.Start(e.Uri.OriginalString);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the Click event of the buttonInfoOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonInfoOK_Click(object sender, RoutedEventArgs e)
        {
            // Close window
            this.Close();
        }

        #endregion // Event handlers

        #region Methods

        /// <summary>
        /// Gets a picture from a file to be assigned to a button or label.
        /// </summary>
        /// <param name="fileName">Name of the file with the picture.</param>
        /// <returns></returns>
        private Image GetImageFromFile(string fileName)
        {
            // Create Icon for button
            Uri uri = new Uri(fileName, UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(uri);
            Image image = new Image();
            image.Source = bitmap;
            image.Stretch = Stretch.Uniform;
            return image;
        }

        #endregion // Methods
    }
}
