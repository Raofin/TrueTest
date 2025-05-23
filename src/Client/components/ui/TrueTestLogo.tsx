'use client'

interface LogoProps {
    size?: string | number; 
    textsz?: string ;
  }

export default function Logo({ size, textsz }: LogoProps){
    const svgSize = size ?? 28;
    const width =  svgSize;
    const height =  svgSize;
    const text= textsz ?? 'text-xl'
    return(
        <div className="flex items-center gap-1 ml-2 ">
          <svg width={width} height={height} viewBox="0 0 28 28" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M14 28C21.732 28 28 21.732 28 14C28 6.26801 21.732 -3.8147e-06 14 -3.8147e-06C6.26802 -3.8147e-06 0 6.26801 0 14C0 21.732 6.26802 28 14 28Z" fill="white"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M24.9758 15.8215L15.823 9.53185L22.3201 6.57755C23.0451 7.39382 23.6573 8.32672 24.1325 9.36936C25.0889 11.4625 25.3337 13.7053 24.9758 15.8215ZM16.1985 24.8839L16.9464 17.8194L6.77857 22.4417C9.34297 24.6256 12.8229 25.5635 16.1985 24.8839ZM17.5946 24.5074C17.9342 24.3898 18.2743 24.2592 18.6043 24.109C21.6269 22.7364 23.7381 20.172 24.6407 17.2216L18.7858 13.1977L17.5946 24.5074ZM5.73594 21.4314L12.2245 18.4856L3.06287 11.9208C2.9342 12.5974 2.8679 13.2895 2.86737 13.9856V13.999C2.86737 15.5333 3.185 17.09 3.8601 18.5758C4.34929 19.6459 4.988 20.6017 5.73594 21.4314ZM10.3033 3.47464C9.99965 3.58285 9.69647 3.70102 9.39282 3.83617C6.44248 5.1774 4.35826 7.65658 3.42134 10.5196L9.24326 14.6936L10.3033 3.47464ZM11.7628 3.11613L11.0193 10.1801L21.1872 5.55891C18.6188 3.37442 15.1384 2.4365 11.7628 3.11613Z" fill="#FF4653"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M9.88391 7.82166C3.77828 11.9208 6.70868 21.422 13.9995 21.422C21.2859 21.422 24.2247 11.9273 18.1156 7.82166C15.646 6.16275 12.3521 6.16371 9.88391 7.82166Z" fill="#FEFEFE"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M10.8881 9.32996C6.2729 12.4284 8.48874 19.61 13.9995 19.61C19.5068 19.61 21.7286 12.4333 17.1104 9.32996C15.245 8.07541 12.7544 8.07642 10.8881 9.32996Z" fill="#D5D5D7"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M11.5169 10.2738C7.83454 12.7455 9.60218 18.4761 13.9995 18.4761C18.3938 18.4761 20.1665 12.75 16.4816 10.2738C14.9923 9.27358 13.0057 9.27358 11.5169 10.2738Z" fill="#5F5F5F"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M11.8096 10.7117C8.56055 12.8931 10.1198 17.9491 13.9995 17.9491C17.8768 17.9491 19.4404 12.8971 16.1895 10.7117C14.8761 9.8296 13.123 9.83055 11.8096 10.7117Z" fill="#434443"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M12.7838 12.1761C10.9819 13.3857 11.8469 16.1909 13.9995 16.1909C16.1501 16.1909 17.0181 13.3877 15.2141 12.1761C14.4852 11.6864 13.5129 11.6864 12.7838 12.1761Z" fill="#22201E"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M12.6163 13.2616C11.7647 12.6907 12.1736 11.3649 13.1912 11.3649C14.2079 11.3649 14.6178 12.6896 13.7652 13.2616C13.4206 13.4934 12.9609 13.4934 12.6163 13.2616Z" fill="#BABCBE"/>
                    <path fillRule="evenodd" clipRule="evenodd" d="M14.9807 16.164C14.5614 15.8828 14.7629 15.2311 15.263 15.2311C15.7636 15.2311 15.965 15.8828 15.5452 16.164C15.3767 16.2787 15.1499 16.2787 14.9807 16.164Z" fill="#FEFEFE"/>
                    </svg>
        <p className={`font-extrabold ${text}`}>
            <span className="text-red-500">True</span>
            <span className="text-blue-500">Test</span>
        </p>
    </div>
    )
}