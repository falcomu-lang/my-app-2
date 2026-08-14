Option Strict Off
Option Explicit On

Module LSI8181
    Public irq_count As Integer = 0
    Public Sub IRQ_testprocess(ByVal Cardid As Byte)
        'when interrupt happening , irq_count + 1
        irq_count = irq_count + 1

    End Sub
    Public IsIoAutoRun As Boolean = False
    Public IsOpenIoFormShow As Boolean = False
    Public IsOpenInterruptFormShow As Boolean = False
    Public Const CMP_OUT = 7
    Public Const CLEAR_IN = 4
    Public Const HOME = 3
    Public Const Z_phase = 2
    Public Const B_phase = 1
    Public Const A_phase = 0

    Public Const IO = 0
    Public Const TIMER_COUNTER = 1
    Public Status As UInteger

    Public Const LSI8181_CARD_ID_MAX = 15
    Public Const LSI8181_IN_POINT_MAX = 7
    Public Const LSI8181_OUT_POINT_MAX = 7
    Public Const I_PORT0 = 0
    Public CardID As Byte
    Public Const O_PORT0 = 1

    '//////// Error Code ///////////////////////
    Public Const DRV_NO_ERROR = 0

    '/************ Driver Error ***************/
    Public Const DRV_INIT_ERROR = 2

    '/************ Device Error ***************/
    Public Const DEVICE_IO_ERROR = 100
    Public Const NO_CARD = 101
    Public Const DUPLICATE_ID = 102

    '/************ User Parameter Error ********/
    Public Const LSI_ID_ERROR = 300
    Public Const LSI_COUNTER_MODE_ERROR = 301
    Public Const LSI_TIMER_CONSTANT_ERROR = 302
    Public Const LSI_CI_MODE_ERROR = 303
    Public Const LSI_MULTIPLE_RATE_ERROR = 304
    Public Const LSI_POINT_ERROR = 305
    Public Const LSI_CO_ERROR = 306
    Public Const LSI_HOME_MODE_ERROR = 307
    Public Const LSI_COMPARE_MODE_ERROR = 308
    Public Const LSI_POLARITY_ERROR = 309

    Public Const LSI_INCREMENT_ERROR = 310
    Public Const LSI_COMPARE_OUT_MODE_ERROR = 311
    Public Const LSI_FIFO_FULL_ERROR = 312
    Public Const LSI_FIFO_EMPTY_ERROR = 313
    Public Const LSI_FIFO_ERROR = 314
    Public Const LSI_THRESHOLD_ERROR = 315
    Public Const LSI_COUNTER_ERROR = 316
    Public Const LSI_IRQ_MASK_ERROR = 317
    Public Const LSI_DRIVER_NOT_SUPPORT = 400

    '//-----------------------------------------------
    Public Const PORT_ERROR = 500
    Public Const DEBOUNCE_MODE_ERROR = 501
    Public Const INDEX_ERROR = 502
    Public Const SOURCE_ERROR = 503


	'/************ DIO ************************/
	Public Const INPORT0_POL = 0
	Public Const OUTPORT0_POL = 1
	Public Const INPORT0 = 0
	Public Const OUTPORT0 = 1
	Public Const CMP_OUT_LOW = 0
	Public Const CMP_OUT_HI = 1

	'/************ CI *************************/
	Public Const QUADRATURE_MODE = 0
	Public Const DUAL_PULSE_MODE = 1
	Public Const SINGLE_PULSE_MODE = 2
	Public Const CI_MAX = 2
	Public Const IN_MODE_MASK = &H3
	Public Const DEBOUNCE_MASK = &HE0
	Public Const DEBOUNCE_MAX = 4
	Public Const MULTIPLE_4 = 0
	Public Const MULTIPLE_2 = 1
	Public Const MULTIPLE_1 = 2

	'//************CO**************************/
	Public Const NO_GATE = &H0
	Public Const GATE = &H1
	Public Const SET_GATE = &H80
	Public Const MODE_MAX = &H0007
	Public Const HOMING_MASK = &H000F
	Public Const SINGLE_MASK = &H0010

	'//***********FIFO*************************/
	Public Const RELATIVE_MODE = 0
	Public Const ABSOLUTE_MODE = 1
	Public Const POSITIVE = 0
	Public Const NEGATIVE = 1
	Public Const TOTAL = &H3FF
	Public Const WCNT_EMPTY = &H0

	'//***********counter start*****************/
	Public Const COUNTER_STOP = 0
	Public Const COUNTER_RUN = 1
	Public Const COUNTER_CMP = 2
	Public Const COUNTER_FIFO = 3
	Public Const COUNTER_CMP_OUT = 4
	Public Const CMP_RUN = &H002
	Public Const CMP_FIFO = &H004
	Public Const CMP_OUT_RUN = &H008
	Public Const C_RUN = &H009
	Public Const C_MASK = &H00E
	Public Const COMP_ = &H00B
	Public Const COMP_FIFO = &H007
	Public Const COMP_OUT = &H00F
	Public Const NEGETIVE = &HFFFF

	'/************ DEBOUNCE_TIME *************/
	Public Const NO_DEBOUNCE_TIME = 0
	Public Const DEBOUNCE_TIME_100HZ = 1
	Public Const DEBOUNCE_TIME_200HZ = 2
	Public Const DEBOUNCE_TIME_1KHZ = 3

	'/************ TIMER **********************/
	Public Const TC_CONTROL = 0
	Public Const PRELOAD = 1
	Public Const COUNTER = 2
    Public Const STOP_ = 0
	Public Const RUN = 1
    Public Const IO_ = 0
	Public Const TC = 1
	Public Const PCI_ENABLE = 2

	'/*================== Input/Output ===================*/
	Public Const IO_PORT_MAX = 1
	Public Const IO_PORT_MIN = 0
	Public Const IO_POINT_MAX = 7
	Public Const IO_DEBOUNCE_MAX = 3
	Public Const IO_STATE = 1

	'/*====================== Timer ======================*/
	Public Const TC_INDEX_MAX = 2

	'/*=============== Quadrature counter ================*/

	'/* CIO */
	Public Const CIO_POL_MAX = &H1FF

	'/* CI */
	Public Const CI_MODE_MAX = 2
	Public Const CI_DEBOUNCE_MAX = 6
	Public Const CI_MULTI_RATE_MAX = 2

	'/* CO */
	Public Const CO_MODE_MAX = 4
	Public Const CO_GATE_MAX = 1

	'/* TOGGLE */
	Public Const TOGGLE_PRESET_MAX = 1

	'/*==================== Homing ======================*/
	Public Const HOMING_MODE_MAX = 7
	Public Const HOMING_SELECT_MAX = 1

	'/*==================== Compare =====================*/
	Public Const CMP_MODE_MAX = 2

	'/* CMP_OUT */
	Public Const CMP_OUT_MODE_MAX = 4
	Public Const CMP_OUT_POL_MAX = 1

	'/* CMP_FIFO */
	Public Const CMP_FIFO_SELECT_MAX = 1
	Public Const CMP_FIFO_SIZE_MAX = 1024
	Public Const CMP_FIFO_SIZE_MIN = 1
	Public Const CMP_FIFO_THRESHOLD_MAX = 1023
	Public Const CMP_FIFO_THRESHOLD_MIN = 1
	Public Const VALUE_MAX = &HF
	Public Const CONTROL_MASK = &H1

	'/* COUNT */
	Public Const COUNT_MODE_MAX = 2

	'/*===== Compare segment configuration and compare out mask off =====*/

	'/* GATE */
	Public Const CMP_GATE_POL_MAX = 1

	'/* SEGMENT */
	Public Const CMP_SEG_MAX = 2
	Public Const CMP_SEG_ATTRI_MAX = 1
	Public Const CMP_SEG_CTL_MAX = 1

	'/*============ Position offset compare ==============*/
	Public Const CMP_POS_CHANNEL_MAX = 7
	Public Const CMP_POS_POINT_MAX = 7
	Public Const CMP_POS_STATE_MAX = 1

	'/*===================== Interrupt ===================*/
	Public Const IRQ_SRC_MAX = 1

	'//-------------------------------------------------------------
	Public Const cDeviceAddress = 6
	Public Const SUCCESS = 0
	Public Const STATUS_SUCCESS = 1



	'//************* card open/close******************************************************/
    Public Declare Function LSI8181_initial Lib "LSI8181_64.DLL" () As Int32
    Public Declare Function LSI8181_close Lib "LSI8181_64.DLL" () As Int32
    Public Declare Function LSI8181_info Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef IO_address As UInt64, ByRef TC_address As UInt64) As Int32

    '//--------------------------DIO--------------------------------------------------------
    Public Declare Function LSI8181_port_polarity_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal port As Byte, ByVal polarity As Byte) As Int32
    Public Declare Function LSI8181_port_polarity_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal port As Byte, ByRef polarity As Byte) As Int32
    Public Declare Function LSI8181_point_polarity_set Lib "LSI8181_64.dll" (ByVal CardID As Byte, ByVal port As Byte, byval point as byte, ByVal polarity As Byte) As Int32
    Public Declare Function LSI8181_point_polarity_read Lib "LSI8181_64.dll" (ByVal CardID As Byte, ByVal port As Byte, byval point as byte, ByRef polarity As Byte) As Int32
    Public Declare Function LSI8181_debounce_time_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal debounce_mode As Byte) As Int32
    Public Declare Function LSI8181_debounce_time_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef debounce_mode As Byte) As Int32
    Public Declare Function LSI8181_port_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal port As Byte, ByVal data As Byte) As Int32
    Public Declare Function LSI8181_port_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal port As Byte, ByRef state As Byte) As Int32
    Public Declare Function LSI8181_point_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal port As Byte, ByVal point As Byte, ByVal state As Byte) As Int32
    Public Declare Function LSI8181_point_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal port As Byte, ByVal point As Byte, ByRef state As Byte) As Int32

    '//----------------------------TC-------------------------------------------------------
    Public Declare Function LSI8181_timer_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal time_constant As UInt32) As Int32

    Public Declare Function LSI8181_timer_start Lib "LSI8181_64.DLL" (ByVal CardID As Byte) As Int32
    Public Declare Function LSI8181_timer_stop Lib "LSI8181_64.DLL" (ByVal CardID As Byte) As Int32
    Public Declare Function LSI8181_TC_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal index As Byte, ByVal data As UInt32) As Int32
    Public Declare Function LSI8181_TC_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal index As Byte, ByRef data As UInt32) As Int32
    Public Declare Function LSI8181_CIO_polarity_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal polarity As UInt16) As Int32
    Public Declare Function LSI8181_CIO_polarity_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef polarity As UInt16) As Int32
    Public Declare Function LSI8181_CIO_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef CIO_state As Byte) As Int32
    Public Declare Function LSI8181_CI_mode_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal in_mode As Byte, ByVal debounce_time As Byte, ByVal multiple_rate As Byte) As Int32
    Public Declare Function LSI8181_CI_mode_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef in_mode As Byte, ByRef debounce_time As Byte, ByRef multiple_rate As Byte) As Int32
    Public Declare Function LSI8181_CO_mode_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal out_mode As Byte, ByVal gate As Byte, ByVal out_width As UInt16) As Int32
    Public Declare Function LSI8181_CO_mode_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef out_mode As Byte, ByRef gate As Byte, ByRef out_width As UInt16) As Int32

    '//-------------------homing & Compare-----------------------------------------------------------
    Public Declare Function LSI8181_HOMING_mode_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal homing_mode As Byte, ByVal z_count As UInt16, ByVal single_cont As Byte) As Int32
    Public Declare Function LSI8181_HOMING_mode_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef homing_mode As Byte, ByRef z_count As UInt16, ByRef single_cont As Byte) As Int32
    Public Declare Function LSI8181_compare_mode_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal compare_mode As Byte) As Int32
    Public Declare Function LSI8181_compare_mode_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef compare_mode As Byte) As Int32
    Public Declare Function LSI8181_counter_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal counter_value As Int32) As Int32
    Public Declare Function LSI8181_counter_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef counter_value As Int32) As Int32
    Public Declare Function LSI8181_compare_value_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal compare_value As Int32) As Int32
    Public Declare Function LSI8181_compare_value_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef compare_value As Int32) As Int32
    Public Declare Function LSI8181_compare_increment_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal increment_value As Int32) As Int32
    Public Declare Function LSI8181_compare_increment_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef increment_value As Int32) As Int32
    Public Declare Function LSI8181_compare_FIFO_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal FIFO_data() As Int32, ByVal rel_abs As Byte, ByVal size As UInt16) As Int32
    Public Declare Function LSI8181_compare_FIFO_threshold_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal threshold_value As UInt16) As Int32
    Public Declare Function LSI8181_compare_FIFO_threshold_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef threshold_value As UInt16) As Int32
    Public Declare Function LSI8181_compare_FIFO_unused_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef unused_count As UInt16) As Int32
    Public Declare Function LSI8181_compare_FIFO_clear Lib "LSI8181_64.DLL" (ByVal CardID As Byte) As Int32
    Public Declare Function LSI8181_counter_start Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal mode As Byte) As Int32
    Public Declare Function LSI8181_counter_stop Lib "LSI8181_64.DLL" (ByVal CardID As Byte) As Int32
    Public Declare Function LSI8181_counter_mode_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef mode As Byte) As Int32

    '//--------------------compare CMP and GATE-------------------------------------------------
    Public Declare Function LSI8181_compare_CMP_OUT_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal polarity As Byte, ByVal out_mode As Byte, ByVal out_width As UInt16) As Int32
    Public Declare Function LSI8181_compare_CMP_OUT_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef polarity As Byte, ByRef out_mode As Byte, ByRef out_width As UInt16) As Int32
    Public Declare Function LSI8181_compare_GATE_enable Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal polarity As Byte) As Int32
    Public Declare Function LSI8181_compare_GATE_disable Lib "LSI8181_64.DLL" (ByVal CardID As Byte) As Int32

    '//--------------------Compare offset --------------------------------------------------------------
    Public Declare Function LSI8181_compare_offset_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal channel As Byte, ByVal offset As Int16) As Int32
    Public Declare Function LSI8181_compare_offset_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal channel As Byte, ByRef offset As Int16) As Int32
    Public Declare Function LSI8181_compare_offset_out_width_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal channel As Byte, ByVal out_width As UInt16) As Int32
    Public Declare Function LSI8181_compare_offset_out_width_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal channel As Byte, ByRef out_width As UInt16) As Int32
    Public Declare Function LSI8181_compare_offset_mask_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal mask As Byte) As Int32
    Public Declare Function LSI8181_compare_offset_mask_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef mask As Byte) As Int32
    Public Declare Function LSI8181_compare_offset_output_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal data As Byte) As Int32
    Public Declare Function LSI8181_compare_offset_output_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef data As Byte) As Int32
    Public Declare Function LSI8181_compare_offset_output_point_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal point As Byte, ByVal state As Byte) As Int32
    Public Declare Function LSI8181_compare_offset_output_point_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal point As Byte, ByRef state As Byte) As Int32


    '//-------------------Compare out trigger ----------------------------------------------------------//20090313
    Public Declare Function LSI8181_segment_control_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal index As Byte, ByRef control As Byte) As Int32
    Public Declare Function LSI8181_segment_control_write Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal index As Byte, ByVal control As Byte) As Int32
    Public Declare Function LSI8181_cmp_segment_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal index As Byte, ByRef start_ As Int32, ByRef stop_ As Int32) As Int32
    Public Declare Function LSI8181_cmp_segment_write Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal index As Byte, ByVal start_ As Int32, ByVal stop_ As Int32) As Int32
    Public Declare Function LSI8181_mask_off_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef attribute1 As Byte) As Int32
    Public Declare Function LSI8181_mask_off_write Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal attribute1 As Byte) As Int32

    '//--------------------Interrupt--------------------------------------------------------
    Public Declare Function LSI8181_IRQ_mask_set Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal source As Byte, ByVal mask As Byte) As Int32
    Public Declare Function LSI8181_IRQ_mask_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal source As Byte, ByRef mask As Byte) As Int32
    Public Declare Function LSI8181_IRQ_process_link Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal callbackAddr As IRQ_PROCESS_Delegate) As Int32
    Public Declare Function LSI8181_IRQ_enable Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef phEvent As Long) As Int32
    Public Declare Function LSI8181_IRQ_disable Lib "LSI8181_64.DLL" (ByVal CardID As Byte) As Int32
    Public Declare Function LSI8181_IRQ_status_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal source As Byte, ByRef event_status As Byte) As Int32

    '//-------------------------------------------------------------------------------------
    Public Declare Function LSI8181_CO_read Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByRef compare_out As Byte) As Int32
    Public Declare Function LSI8181_toggle_preset Lib "LSI8181_64.DLL" (ByVal CardID As Byte, ByVal preset As Byte) As Int32

    Public Delegate Sub IRQ_PROCESS_Delegate(ByVal CardID As Byte)

End Module
